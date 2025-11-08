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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SolutionVersionHandler.Controls
{
  /// <summary>
  /// Interaction logic for VersionEditor.xaml
  /// </summary>
  public partial class VersionEditor : UserControl
  {
    public delegate void SpreadDelegate(Model.Version version);
    public event SpreadDelegate? SpreadAcrossProjectRequested;
    public event SpreadDelegate? SpreadAcrossVersionTypeRequested;
    public event Action? RemoveVersionRequested;

    public static DependencyProperty VersionProperty = DependencyProperty.Register(
      nameof(Version),
      typeof(Model.Version),
      typeof(VersionEditor),
      new PropertyMetadata(null));

    public VersionEditor()
    {
      InitializeComponent();
    }

    public Model.Version Version
    {
      get { return (Model.Version)this.GetValue(VersionProperty); }
      set
      {
        this.SetValue(VersionProperty, value);
        this.DataContext = value;
      }
    }

    private void btnSpreadAcrossProjects_Click(object sender, RoutedEventArgs e)
    {
      this.SpreadAcrossProjectRequested?.Invoke(this.Version);
    }

    private void btnSpreadAcrossVersionTypes_Click(object sender, RoutedEventArgs e)
    {
      this.SpreadAcrossVersionTypeRequested?.Invoke(this.Version);
    }

    private void btnRemoveVersion_Click(object sender, RoutedEventArgs e)
    {
      this.RemoveVersionRequested?.Invoke();
    }
  }
}
