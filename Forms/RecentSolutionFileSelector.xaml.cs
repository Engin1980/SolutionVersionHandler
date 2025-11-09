using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using SolutionVersionHandler.Model;

namespace SolutionVersionHandler.Forms
{
  /// <summary>
  /// Interaction logic for RecentSolutionFileSelector.xaml
  /// </summary>
  public partial class RecentSolutionFileSelector : Window
  {
    public static readonly DependencyProperty SelectedSolutionFileProperty = DependencyProperty.Register(
      nameof(SelectedSolutionFile),
      typeof(AppViewModel.RecentSolutionFile),
      typeof(RecentSolutionFileSelector),
      new PropertyMetadata(null));

    public AppViewModel.RecentSolutionFile? SelectedSolutionFile
    {
      get => (AppViewModel.RecentSolutionFile?)GetValue(SelectedSolutionFileProperty);
      set => SetValue(SelectedSolutionFileProperty, value);
    }

    public RecentSolutionFileSelector()
    {
      InitializeComponent();

      this.DataContext = AppViewModel.Instance;
    }

    private void btnOK_Click(object sender, RoutedEventArgs e)
    {
      if (cmbRecentSolutions.SelectedItem is AppViewModel.RecentSolutionFile rsf)
      {
        SelectedSolutionFile = rsf;
        this.DialogResult = true;
      }
    }

    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
      this.DialogResult = false;
    }
  }
}
