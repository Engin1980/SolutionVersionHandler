using ESystem.Asserting;
using SolutionVersionHandler.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SolutionVersionHandler.Services;

internal static class LoadSaveProvider
{
  internal static Solution LoadSolution(string filePath)
  {
    Solution ret = new Analyser().AnalyseSolution(filePath);
    return ret;
  }

  internal static void SaveProjects(System.ComponentModel.BindingList<Project> projects)
  {
    foreach (Project proj in projects)
    {
      SaveProject(proj);
    }
  }

  private static void SaveProject(Project proj)
  {
    XDocument doc;
    try
    {
      doc = XDocument.Load(proj.FilePath)
        ?? throw new ApplicationException("Project file contains no document: " + proj.FilePath);
      if (doc.Root == null) throw new ApplicationException("Project file has no root element: " + proj.FilePath);

      var propertyGroups = doc.Root.Elements("PropertyGroup");
      foreach (var pg in propertyGroups)
      {
        SetVersionElement(pg, "AssemblyVersion", proj.AssemblyVersion);
        SetVersionElement(pg, "FileVersion", proj.FileVersion);
        SetVersionElement(pg, "PackageVersion", proj.PackageVersion);
        SetVersionElement(pg, "Version", proj.Version);
      }
    }
    catch (Exception ex)
    {
      throw new ApplicationException("Failed to adjust project proj XML file : " + proj.FilePath, ex);
    }

    try
    {
      string tempFile = System.IO.Path.GetTempFileName();
      System.IO.File.Copy(proj.FilePath, proj.FilePath + ".versions.bak", true);
      doc.Save(tempFile);
      System.IO.File.Copy(tempFile, proj.FilePath, true);
      System.IO.File.Delete(tempFile);
    }
    catch (Exception ex)
    {
      throw new ApplicationException("Failed to save adjusted project file: " + proj.FilePath, ex);
    }
  }
  private static void SetVersionElement(XElement pg, string elementName, Model.Version? version)
  {
    // if version == null, remove potetinal subelement from pg
    if (version == null)
    {
      var elToRemove = pg.Element(elementName);
      elToRemove?.Remove();
      return;
    }
    else
    {
      string value = version.Unparseable ?? version.FullVersion;
      var el = pg.Element(elementName);
      if (el == null)
        pg.Add(new XElement(elementName, value));
      else
        el.Value = value;
    }
  }
}