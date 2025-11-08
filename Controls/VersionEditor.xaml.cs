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

namespace SolutionVersionHandler
{
  /// <summary>
  /// Interaction logic for VersionEditor.xaml
  /// </summary>
  public partial class VersionEditor : UserControl
  {
    // legacy CLR events (kept for compatibility)
    public delegate void SpreadDelegate(Model.Version version);
    public event SpreadDelegate? SpreadAcrossProjectRequested;
    public event SpreadDelegate? SpreadAcrossVersionTypeRequested;
    public event Action? RemoveVersionRequested;

    // DependencyProperty for Version
    public static readonly DependencyProperty VersionProperty = DependencyProperty.Register(
      nameof(Version),
      typeof(Model.Version),
      typeof(VersionEditor),
      new PropertyMetadata(null));

    // ICommand DPs (new) - prefer these in MVVM usage
    public static readonly DependencyProperty SpreadAcrossProjectsCommandProperty = DependencyProperty.Register(
      nameof(SpreadAcrossProjectsCommand),
      typeof(ICommand),
      typeof(VersionEditor),
      new PropertyMetadata(null));

    public static readonly DependencyProperty SpreadAcrossVersionTypesCommandProperty = DependencyProperty.Register(
      nameof(SpreadAcrossVersionTypesCommand),
      typeof(ICommand),
      typeof(VersionEditor),
      new PropertyMetadata(null));

    public static readonly DependencyProperty RemoveVersionCommandProperty = DependencyProperty.Register(
      nameof(RemoveVersionCommand),
      typeof(ICommand),
      typeof(VersionEditor),
      new PropertyMetadata(null));

    // Optional command parameter DPs (if caller wants to pass something other than Version)
    public static readonly DependencyProperty SpreadAcrossProjectsCommandParameterProperty = DependencyProperty.Register(
      nameof(SpreadAcrossProjectsCommandParameter),
      typeof(object),
      typeof(VersionEditor),
      new PropertyMetadata(null));

    public static readonly DependencyProperty SpreadAcrossVersionTypesCommandParameterProperty = DependencyProperty.Register(
      nameof(SpreadAcrossVersionTypesCommandParameter),
      typeof(object),
      typeof(VersionEditor),
      new PropertyMetadata(null));

    public static readonly DependencyProperty RemoveVersionCommandParameterProperty = DependencyProperty.Register(
      nameof(RemoveVersionCommandParameter),
      typeof(object),
      typeof(VersionEditor),
      new PropertyMetadata(null));

    public VersionEditor()
    {
      InitializeComponent();
    }

    public Model.Version Version
    {
      get { return (Model.Version)this.GetValue(VersionProperty); }
      set { this.SetValue(VersionProperty, value); }
    }

    // ICommand CLR wrappers
    public ICommand? SpreadAcrossProjectsCommand
    {
      get => (ICommand?)GetValue(SpreadAcrossProjectsCommandProperty);
      set => SetValue(SpreadAcrossProjectsCommandProperty, value);
    }

    public ICommand? SpreadAcrossVersionTypesCommand
    {
      get => (ICommand?)GetValue(SpreadAcrossVersionTypesCommandProperty);
      set => SetValue(SpreadAcrossVersionTypesCommandProperty, value);
    }

    public ICommand? RemoveVersionCommand
    {
      get => (ICommand?)GetValue(RemoveVersionCommandProperty);
      set => SetValue(RemoveVersionCommandProperty, value);
    }

    // Optional command parameters
    public object? SpreadAcrossProjectsCommandParameter
    {
      get => GetValue(SpreadAcrossProjectsCommandParameterProperty);
      set => SetValue(SpreadAcrossProjectsCommandParameterProperty, value);
    }

    public object? SpreadAcrossVersionTypesCommandParameter
    {
      get => GetValue(SpreadAcrossVersionTypesCommandParameterProperty);
      set => SetValue(SpreadAcrossVersionTypesCommandParameterProperty, value);
    }

    public object? RemoveVersionCommandParameter
    {
      get => GetValue(RemoveVersionCommandParameterProperty);
      set => SetValue(RemoveVersionCommandParameterProperty, value);
    }

    private void btnSpreadAcrossProjects_Click(object sender, RoutedEventArgs e)
    {
      // Try command first
      if (!ExecuteCommand(SpreadAcrossProjectsCommand, SpreadAcrossProjectsCommandParameter ?? this.Version))
      {
        // fallback to legacy event
        this.SpreadAcrossProjectRequested?.Invoke(this.Version);
      }
    }

    private void btnSpreadAcrossVersionTypes_Click(object sender, RoutedEventArgs e)
    {
      if (!ExecuteCommand(SpreadAcrossVersionTypesCommand, SpreadAcrossVersionTypesCommandParameter ?? this.Version))
      {
        this.SpreadAcrossVersionTypeRequested?.Invoke(this.Version);
      }
    }

    private void btnRemoveVersion_Click(object sender, RoutedEventArgs e)
    {
      if (!ExecuteCommand(RemoveVersionCommand, RemoveVersionCommandParameter ?? this.Version))
      {
        this.RemoveVersionRequested?.Invoke();
      }
    }

    private bool ExecuteCommand(ICommand? command, object? parameter)
    {
      if (command == null) return false;
      try
      {
        if (command.CanExecute(parameter))
        {
          command.Execute(parameter);
          return true;
        }
      }
      catch
      {
        // swallow exceptions from commands to avoid breaking UI; caller should handle errors
      }
      return false;
    }
  }
}
