using SolutionVersionHandler.Model;
using System.Configuration;
using System.Data;
using System.Windows;

namespace SolutionVersionHandler
{
  /// <summary>
  /// Interaction logic for App.xaml
  /// </summary>
  public partial class App : Application
  {
    public static string RecentSolutionFilesFile => System.IO.Path.Combine(
      Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
      "SolutionVersionHandler",
      "recent_solutions.txt");
    protected override void OnStartup(StartupEventArgs e)
    {
      base.OnStartup(e);
      LoadRecentSolutionFiles();
    }

    private void LoadRecentSolutionFiles()
    {
      if (System.IO.File.Exists(RecentSolutionFilesFile))
      {
        var lines = System.IO.File.ReadAllLines(RecentSolutionFilesFile);
        lines
          .Select(q => new AppViewModel.RecentSolutionFile(q))
          .ToList()
          .ForEach(q => AppViewModel.Instance.RecentSolutionFiles.Add(q));
      }
    }

    protected override void OnExit(ExitEventArgs e)
    {
      SaveRecentSolutionFiles();
      base.OnExit(e);
    }

    private void SaveRecentSolutionFiles()
    {
      string dir = System.IO.Path.GetDirectoryName(RecentSolutionFilesFile)!;
      if (System.IO.Directory.Exists(dir) == false)
      {
        System.IO.Directory.CreateDirectory(dir);
      }
      var lines = AppViewModel.Instance.RecentSolutionFiles
        .Select(q => q.FilePath)
        .ToArray();
      System.IO.File.WriteAllLines(RecentSolutionFilesFile, lines);
    }
  }

}
