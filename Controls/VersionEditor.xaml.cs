using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SolutionVersionHandler.Model;
using System.ComponentModel;
using System.Printing;

namespace SolutionVersionHandler.Controls
{
  /// <summary>
  /// Interaction logic for VersionEditor.xaml
  /// </summary>
  public partial class VersionEditor : UserControl
  {
    public static readonly DependencyProperty ProjectsProperty = DependencyProperty.Register(
      nameof(Projects),
      typeof(BindingList<Model.Project>),
      typeof(VersionEditor),
      new PropertyMetadata(new BindingList<Model.Project>(), OnDependencyPropertyValueChanged));

    private static void OnDependencyPropertyValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      var me = (VersionEditor)d;
      me.OnDependencyPropertyValueChanged();
    }

    private void OnDependencyPropertyValueChanged()
    {
      if (SelectedProject == null)
        ShownVersion = null;
      else
      {
        var getter = GetGetterByType(VersionType);
        ShownVersion = getter(SelectedProject);
        System.Diagnostics.Debug.WriteLine($"Project {SelectedProject.Name} version {VersionType} changed to: {ShownVersion}");
      }

      if (ShownVersion == null)
      {
        pnlEmpty.Visibility = Visibility.Visible;
        pnlUnparseable.Visibility = pnlVersion.Visibility = Visibility.Collapsed;
      }
      else if (ShownVersion.Unparseable != null)
      {
        pnlUnparseable.Visibility = Visibility.Visible;
        pnlEmpty.Visibility = pnlVersion.Visibility = Visibility.Collapsed;
      }
      else
      {
        pnlVersion.Visibility = Visibility.Visible;
        pnlEmpty.Visibility = pnlUnparseable.Visibility = Visibility.Collapsed;
      }
    }

    private static Func<Project, Model.Version?> GetGetterByType(EVersionType type)
    {
      return type switch
      {
        EVersionType.AssemblyVersion => ((Project p) => p.AssemblyVersion),
        EVersionType.FileVersion => (Project p) => p.FileVersion,
        EVersionType.PackageVersion => (Project p) => p.PackageVersion,
        EVersionType.Version => (Project p) => p.Version,
        _ => throw new NotImplementedException()
      };
    }

    private static Action<Project, Model.Version?> GetSetterByType(EVersionType type)
    {
      return type switch
      {
        EVersionType.AssemblyVersion => ((Project p, Model.Version? v) => p.AssemblyVersion = v),
        EVersionType.FileVersion => ((Project p, Model.Version? v) => p.FileVersion = v),
        EVersionType.PackageVersion => ((Project p, Model.Version? v) => p.PackageVersion = v),
        EVersionType.Version => ((Project p, Model.Version? v) => p.Version = v),
        _ => throw new NotImplementedException()
      };
    }


    public BindingList<Project> Projects
    {
      get { return (BindingList<Project>)GetValue(ProjectsProperty); }
      set { SetValue(ProjectsProperty, value); }
    }

    public static readonly DependencyProperty SelectedProjectProperty = DependencyProperty.Register(
      nameof(SelectedProject),
      typeof(Model.Project),
      typeof(VersionEditor),
      new PropertyMetadata(null, OnDependencyPropertyValueChanged));
    public Model.Project SelectedProject
    {
      get { return (Model.Project)GetValue(SelectedProjectProperty); }
      set { SetValue(SelectedProjectProperty, value); }
    }

    public static readonly DependencyProperty VersionTypeProperty = DependencyProperty.Register(
      nameof(VersionType),
      typeof(EVersionType),
      typeof(VersionEditor),
      new PropertyMetadata(EVersionType.Version, OnDependencyPropertyValueChanged));

    public EVersionType VersionType
    {
      get { return (EVersionType)GetValue(VersionTypeProperty); }
      set { SetValue(VersionTypeProperty, value); }
    }

    private static List<VersionEditor> instances = [];
    public VersionEditor()
    {
      InitializeComponent();
      instances.Add(this);
    }

    public static void ForceRebind()
    {
      foreach (var instance in instances)
        instance.ForceRebindThis();
    }
    public void ForceRebindThis()
    {
      this.OnDependencyPropertyValueChanged();
    }


    private static readonly DependencyProperty ShownVersionProperty = DependencyProperty.Register(
      nameof(ShownVersion),
      typeof(Model.Version),
      typeof(VersionEditor),
      new PropertyMetadata(null, OnDependencyPropertyValueChanged));

    private Model.Version? ShownVersion
    {
      get { return (Model.Version?)GetValue(ShownVersionProperty); }
      set { this.SetValue(ShownVersionProperty, value); }
    }

    private void btnSpreadAcrossProjects_Click(object sender, RoutedEventArgs e)
    {
      var getter = GetGetterByType(VersionType);
      var setter = GetSetterByType(VersionType);
      Model.Version? v = getter(this.SelectedProject)?.Clone();
      var projs = this.Projects.Count(q => q.IsChecked) == 0 ? this.Projects : this.Projects.Where(q => q.IsChecked);
      foreach (var project in Projects)
        setter(project, v);
      ForceRebind();
    }

    private void btnSpreadAcrossVersionTypes_Click(object sender, RoutedEventArgs e)
    {
      var getter = GetGetterByType(VersionType);
      Model.Version? v = getter(this.SelectedProject)?.Clone();
      foreach (EVersionType vt in Enum.GetValues(typeof(EVersionType)))
      {
        var setter = GetSetterByType(vt);
        setter(SelectedProject, v);
      }
      ForceRebind();
    }

    private void btnRemoveVersion_Click(object sender, RoutedEventArgs e)
    {
      var setter = GetSetterByType(VersionType);
      setter(SelectedProject, null);
      ForceRebindThis();
    }

    private void btnNewTextVersion_Click(object sender, RoutedEventArgs e)
    {
      var setter = GetSetterByType(VersionType);
      setter(SelectedProject, new Model.Version { Unparseable = "${?}" });
      ForceRebindThis();
    }

    private void btnNewNumericVersion_Click(object sender, RoutedEventArgs e)
    {
      var setter = GetSetterByType(VersionType);
      setter(SelectedProject, new Model.Version());
      ForceRebindThis();
    }
  }
}
