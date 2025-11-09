using Microsoft.Win32;
using SolutionVersionHandler.Model;
using SolutionVersionHandler.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml.Linq;

namespace SolutionVersionHandler
{
  public partial class MainWindow : Window
  {

    public MainWindow()
    {
      InitializeComponent();

      var vm = AppViewModel.Instance;
      // "?" column is not in the editable list
      vm.Columns.Create(ColumnList.PROJECT_COLUMN_TITLE);
      vm.Columns.Create(ColumnList.PROJECT_PATH_COLUMN_TITLE, false);
      vm.Columns.Create(ColumnList.VERSION_COLUMN_TITLE);
      vm.Columns.Create(ColumnList.ASSEMBLY_VERSION_COLUMN_TITLE);
      vm.Columns.Create(ColumnList.PACKAGE_VERSION_COLUMN_TITLE);
      vm.Columns.Create(ColumnList.FILE_VERSION_COLUMN_TITLE);

      // load solution
      Solution sol = new Analyser().AnalyseSolution(@"D:\repos\eSystem\ESystemNet.sln");
      vm.Projects = sol.Projects;
      this.DataContext = vm;
    }

    private void btnLoadSolution_Click(object sender, RoutedEventArgs e)
    {
      var ofd = new OpenFileDialog
      {
        Filter = "Solution files (*.sln)|*.sln|All files (*.*)|*.*"
      };
      if (ofd.ShowDialog() == true)
      {
        btnLoadSolution.IsEnabled = false;
        try
        {
          Solution sol = new Analyser().AnalyseSolution(ofd.FileName);
          this.DataContext = sol;
        }
        catch (Exception ex)
        {
          MessageBox.Show("Error loading solution: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        finally
        {
          btnLoadSolution.IsEnabled = true;
        }
      }
    }

    private void btnSaveProjects_Click(object sender, RoutedEventArgs e)
    {
      // ask to replace; if confirmed, save all projects
      var result = MessageBox.Show("Are you sure you want to save all project files with the updated versions?", "Confirm Save", MessageBoxButton.YesNo, MessageBoxImage.Question);
      if (result == MessageBoxResult.Yes)
      {
        // temporally only messagebox
        MessageBox.Show("Projects saved successfully.", "Save Complete", MessageBoxButton.OK, MessageBoxImage.Information);

        btnSaveProjects.IsEnabled = false;
        try
        {
          SaveProjects();
        }
        catch (Exception ex)
        {
          MessageBox.Show("Failed to save projects. " + ex.Message);
        }
        finally
        {
          btnSaveProjects.IsEnabled = true;
        }
      }
    }

    private void SaveProjects()
    {
      foreach (Project proj in ((Solution)this.DataContext).Projects)
      {
        SaveProject(proj);
      }
    }

    private void SaveProject(Project proj)
    {
      System.IO.File.Copy(proj.FilePath, proj.FilePath + ".versions.bak", true);

      // load project's xml file
      var doc = System.Xml.Linq.XDocument.Load(proj.FilePath);

      if (doc == null) throw new ApplicationException("Project file contains no document: " + proj.FilePath);
      if (doc.Root == null) throw new ApplicationException("Project file has no root element: " + proj.FilePath);

      // get PropertyGroup
      var propertyGroups = doc.Root.Elements("PropertyGroup");
      foreach (var pg in propertyGroups)
      {
        // set versions
        SetVersionElement(pg, "AssemblyVersion", proj.AssemblyVersion);
        SetVersionElement(pg, "FileVersion", proj.FileVersion);
        SetVersionElement(pg, "PackageVersion", proj.PackageVersion);
        SetVersionElement(pg, "Version", proj.Version);
      }

      doc.Save(proj.FilePath);
    }

    private void SetVersionElement(XElement pg, string elementName, Model.Version? version)
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

    private void cmbColumns_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      cmbColumns.SelectedItem = null;
    }
  }
}