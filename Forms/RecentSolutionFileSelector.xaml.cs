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

    private void btnDeleteRecentSolutionFile_Click(object sender, RoutedEventArgs e)
    {
      // Delete the selected recent solution file from the list
      // before deletion, ask for confirmation
      // The delete button is inside the ComboBox item template; determine the item from the button's DataContext
      if (sender is not Button btn)
        return;

      if (btn.DataContext is not AppViewModel.RecentSolutionFile selected)
        return;

      var message = $"Do you really want to remove the recent solution entry:\n\n{selected.FileName}?";
      var caption = "Confirm delete";
      var result = MessageBox.Show(this, message, caption, MessageBoxButton.YesNo, MessageBoxImage.Question);
      if (result != MessageBoxResult.Yes)
        return;

      var recentList = AppViewModel.Instance?.RecentSolutionFiles;
      if (recentList == null)
        return;

      // Remove the selected item
      recentList.Remove(selected);

      // If the removed item was currently selected in the ComboBox, clear ComboBox selection
      if (Equals(cmbRecentSolutions.SelectedItem, selected))
      {
        cmbRecentSolutions.SelectedItem = null;
      }

      // If the removed item was currently selected in this dialog, clear SelectedSolutionFile
      if (SelectedSolutionFile == selected)
      {
        SelectedSolutionFile = null;
      }
    }
  }
}
