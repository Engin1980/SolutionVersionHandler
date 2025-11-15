using ESystem.Asserting;
using Microsoft.Win32;
using SolutionVersionHandler.Model;
using SolutionVersionHandler.Services;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
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
        AppViewModel.Instance.State.IsBusy = true;
        try
        {
          LoadAndSetSolutionProjects(ofd.FileName);
          UpdateRecentSolutionFilesList(ofd);
          UpdateCheckedColumns();
        }
        catch (Exception ex)
        {
          MessageBox.Show("Error loading solution: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        finally
        {
          AppViewModel.Instance.State.IsBusy = false;
        }
      }
    }

    private void UpdateCheckedColumns()
    {
      AppViewModel.Instance.Columns[ColumnList.VERSION_COLUMN_TITLE].IsChecked =
        AppViewModel.Instance.Projects.Any(q => q.Version != null);
      AppViewModel.Instance.Columns[ColumnList.ASSEMBLY_VERSION_COLUMN_TITLE].IsChecked =
        AppViewModel.Instance.Projects.Any(q => q.AssemblyVersion != null);
      AppViewModel.Instance.Columns[ColumnList.FILE_VERSION_COLUMN_TITLE].IsChecked =
        AppViewModel.Instance.Projects.Any(q => q.FileVersion != null);
      AppViewModel.Instance.Columns[ColumnList.PACKAGE_VERSION_COLUMN_TITLE].IsChecked =
        AppViewModel.Instance.Projects.Any(q => q.PackageVersion != null);
    }

    private static void UpdateRecentSolutionFilesList(OpenFileDialog ofd)
    {
      AppViewModel.RecentSolutionFile recentSolutionFile = new(ofd.FileName);
      if (AppViewModel.Instance.RecentSolutionFiles.Contains(recentSolutionFile) == false)
        AppViewModel.Instance.RecentSolutionFiles.Add(recentSolutionFile);
    }

    private void btnSaveProjects_Click(object sender, RoutedEventArgs e)
    {
      // ask to replace; if confirmed, save all projects
      var result = MessageBox.Show("Are you sure you want to save all project files with the updated versions?", "Confirm Save", MessageBoxButton.YesNo, MessageBoxImage.Question);
      if (result == MessageBoxResult.Yes)
      {
        AppViewModel.Instance.State.IsBusy = true;
        try
        {
          SaveProjects();
          MessageBox.Show("Changes saved.");
        }
        catch (Exception ex)
        {
          MessageBox.Show("Failed to save projects. " + ex.Message);
        }
        finally
        {
          AppViewModel.Instance.State.IsBusy = false;
        }
      }
    }

    private void SaveProjects()
    {
      LoadSaveProvider.SaveProjects(AppViewModel.Instance.Projects);
    }


    private void cmbColumns_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      cmbColumns.SelectedItem = null;
    }

    private void btnLoadRecentSolution_Click(object sender, RoutedEventArgs e)
    {
      var frm = new Forms.RecentSolutionFileSelector();
      frm.Owner = this;
      frm.WindowStartupLocation = WindowStartupLocation.CenterOwner;
      var res = frm.ShowDialog();
      if (res == true && frm.SelectedSolutionFile != null)
      {
        AppViewModel.Instance.State.IsBusy = true;
        try
        {
          LoadAndSetSolutionProjects(frm.SelectedSolutionFile.FilePath);
          UpdateCheckedColumns();
        }
        catch (Exception ex)
        {
          MessageBox.Show("Error loading solution: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        finally
        {
          AppViewModel.Instance.State.IsBusy = false;
        }
      }
    }

    private void LoadAndSetSolutionProjects(string filePath)
    {
      Solution sol = LoadSaveProvider.LoadSolution(filePath);
      AppViewModel.Instance.Projects = sol.Projects;
      AppViewModel.Instance.SolutionFile = filePath;
      AppViewModel.Instance.State.IsSomeSolutionFileSet = true;
    }

    private void btnReloadCurrent_Click(object sender, RoutedEventArgs e)
    {
      var conf = MessageBox.Show(
        "Are you sure you want to reload the current solution? Unsaved changes will be lost.",
        "Confirm Reload",
        MessageBoxButton.YesNo,
        MessageBoxImage.Question);
      if (conf != MessageBoxResult.Yes)
        return;

      EAssert.IsNotNull(AppViewModel.Instance.SolutionFile);
      AppViewModel.Instance.State.IsBusy = true;
      try
      {
        LoadAndSetSolutionProjects(AppViewModel.Instance.SolutionFile);
      }
      catch (Exception ex)
      {
        MessageBox.Show("Error loading solution: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
      }
      finally
      {
        AppViewModel.Instance.State.IsBusy = false;
      }
    }
  }
}