using SolutionVersionHandler.Model;
using SolutionVersionHandler.Services;
using SolutionVersionHandler.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SolutionVersionHandler
{
  public partial class MainWindow : Window
  {
    private readonly MainViewModel _vm;

    public MainWindow()
    {
      InitializeComponent();

      _vm = new MainViewModel();

      // load solution
      Solution sol = new Analyser().AnalyseSolution(@"D:\repos\eSystem\ESystemNet.sln");
      foreach (var p in sol.Projects)
        _vm.Projects.Add(p);

      this.DataContext = _vm;
    }

    private Project? FindAncestorDataGridRowItem(DependencyObject child)
    {
      while (child != null)
      {
        if (child is DataGridRow row)
          return row.Item as Project;
        child = System.Windows.Media.VisualTreeHelper.GetParent(child);
      }
      return null;
    }
  }
}