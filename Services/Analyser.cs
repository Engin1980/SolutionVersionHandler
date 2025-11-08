using ESystem.Asserting;
using SolutionVersionHandler.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SolutionVersionHandler.Services
{
  public class Analyser
  {
    internal Solution AnalyseSolution(string solutionFileName)
    {
      if (string.IsNullOrWhiteSpace(solutionFileName))
        throw new ArgumentNullException(nameof(solutionFileName));

      var solutionPath = Path.GetFullPath(solutionFileName);

      if (!File.Exists(solutionPath))
        throw new FileNotFoundException("Solution file not found.", solutionPath);

      var solutionDir = Path.GetDirectoryName(solutionPath) ?? string.Empty;
      var lines = File.ReadAllLines(solutionPath);

      // Project(...) = "Name", "path\\to\\project.csproj", "{GUID}"
      var projectLines = lines
        .Where(l => l.TrimStart().StartsWith("Project(", StringComparison.OrdinalIgnoreCase));

      var projects = new BindingList<Project>();
      var quoteRegex = new Regex(@"Project\(""(.+)\""\) = ""(.+)"", ""(.+)"", ""(.+)""", RegexOptions.Compiled);

      foreach (var line in projectLines)
      {
        var matches = quoteRegex.Matches(line);
        // Expect at least two quoted strings: project name and project file path
        EAssert.IsTrue(matches.Count <= 1);
        if (matches.Count > 0)
        {
          var match = matches[0];
          var guidSol = match.Groups[1].Value;
          var projName = match.Groups[2].Value;
          var projPathRaw = match.Groups[3].Value;
          var guidProj = match.Groups[4].Value;

          // Normalize separators and resolve relative paths against the solution directory
          var normalized = projPathRaw.Replace('/', Path.DirectorySeparatorChar).Replace('\\', Path.DirectorySeparatorChar);
          var projFullPath = Path.IsPathRooted(normalized)
            ? Path.GetFullPath(normalized)
            : Path.GetFullPath(Path.Combine(solutionDir, normalized));

          Project project = AnalyseProject(projFullPath, projName);
          projects.Add(project);
        }
      }

      return new Solution
      {
        FilePath = solutionPath,
        Name = Path.GetFileNameWithoutExtension(solutionPath),
        Projects = projects
      };
    }

    private Project AnalyseProject(string projFullPath, string projName)
    {
      var project = new Project
      {
        Name = string.IsNullOrWhiteSpace(projName) ? Path.GetFileNameWithoutExtension(projFullPath) : projName,
        FilePath = projFullPath,
        Version = new SolutionVersionHandler.Model.Version()
      };

      if (!File.Exists(projFullPath))
        return project;

      try
      {
        // Load csproj as XML and try to read common version properties
        var doc = XDocument.Load(projFullPath);

        string GetProperty(string localName)
        {
          var el = doc.Descendants().FirstOrDefault(e => string.Equals(e.Name.LocalName, localName, StringComparison.OrdinalIgnoreCase));
          return el?.Value!;
        }

        // Prefer <Version>, then <PackageVersion>, then combination of <VersionPrefix> and <VersionSuffix>
        string? tmp;
        tmp = GetProperty("Version");
        if (!string.IsNullOrWhiteSpace(tmp))
          try
          {
            project.Version = ParseVersion(tmp);
          }
          catch
          {
            project.Version = Model.Version.AsUnparseable(tmp);
          }

        tmp = GetProperty("FileVersion");
        if (!string.IsNullOrWhiteSpace(tmp))
          try
          {
            project.FileVersion = ParseVersion(tmp);
          }
          catch
          {
            project.FileVersion = Model.Version.AsUnparseable(tmp);
          }

        tmp = GetProperty("PackageVersion");
        if (!string.IsNullOrWhiteSpace(tmp))
          try
          {
            project.PackageVersion = ParseVersion(tmp);
          }
          catch
          {
            project.PackageVersion = Model.Version.AsUnparseable(tmp);
          }

        tmp = GetProperty("AssemblyVersion");
        if (!string.IsNullOrWhiteSpace(tmp))
          try
          {
            project.AssemblyVersion = ParseVersion(tmp);
          }
          catch
          {
            project.AssemblyVersion = Model.Version.AsUnparseable(tmp);
          }

        project.VersionPrefix = GetProperty("VersionPrefix") ?? null;
        project.VersionSuffix = GetProperty("VersionSuffix") ?? null;
      }
      catch
      {
        // suppress any parse errors and return what we have
      }

      return project;
    }

    private Model.Version ParseVersion(string versionString)
    {
      versionString = versionString.Trim();
      versionString = versionString.Trim('"');

      var numericPart = versionString;

      // Match up to four numeric groups
      var verMatch = Regex.Match(numericPart, "^(\\d+)(?:\\.(\\d+))?(?:\\.(\\d+))?(?:\\.(\\d+))?");
      if (verMatch.Success)
      {
        int ParseGroup(int idx)
        {
          if (verMatch.Groups.Count > idx && int.TryParse(verMatch.Groups[idx].Value, out var v))
            return v;
          return 0;
        }

        return new Model.Version()
        {
          Major = ParseGroup(1),
          Minor = ParseGroup(2),
          Build = ParseGroup(3),
          Revision = ParseGroup(4)
        };
      }
      else
      {
        throw new ApplicationException($"Unable to parse version string '{versionString}'.");
      }
    }
  }
}
