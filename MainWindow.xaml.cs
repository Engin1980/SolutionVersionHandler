using SolutionVersionHandler.Model;
using SolutionVersionHandler.Services;
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

    public MainWindow()
    {
      InitializeComponent();

      // load solution
      Solution sol = new Analyser().AnalyseSolution(@"D:\repos\eSystem\ESystemNet.sln");
      this.DataContext = sol;
    }
  }
}