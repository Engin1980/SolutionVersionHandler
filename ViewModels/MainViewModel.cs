using SolutionVersionHandler.Model;
using SolutionVersionHandler.Services;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace SolutionVersionHandler.ViewModels
{
  internal class MainViewModel
  {
    public ObservableCollection<Project> Projects { get; } = new ObservableCollection<Project>();

    public ICommand SpreadAcrossProjectsCommand { get; }
    public ICommand SpreadAcrossVersionTypesCommand { get; }
    public ICommand RemoveVersionCommand { get; }

    public MainViewModel()
    {
      SpreadAcrossProjectsCommand = new RelayCommand(param => OnSpreadAcrossProjects(param));
      SpreadAcrossVersionTypesCommand = new RelayCommand(param => OnSpreadAcrossVersionTypes(param));
      RemoveVersionCommand = new RelayCommand(param => OnRemoveVersion(param));
    }

    private void OnSpreadAcrossProjects(object? parameter)
    {
      var pav = parameter as ProjectAndVersion;
      var project = pav?.Project ?? parameter as Project;
      var version = pav?.Version ?? parameter as SolutionVersionHandler.Model.Version;

      if (project == null) throw new ApplicationException("Unexpected null.");
      if (version == null) throw new ApplicationException("Unexpected null.");

      Action<Project, Model.Version> adjuster;
      if (project.AssemblyVersion == version)
        adjuster = (p, v) => p.AssemblyVersion = v;
      else if (project.FileVersion == version)
        adjuster = (p, v) => p.FileVersion = v;
      else if (project.PackageVersion == version)
        adjuster = (p, v) => p.PackageVersion = v;
      else if (project.Version == version)
        adjuster = (p, v) => p.Version = v;
      else
        throw new ApplicationException("Unknown version of the project.");

      foreach (var p in Projects)
      {
        if (p == null) continue;
        adjuster(p, (Model.Version)version.Copy()!);
      }
    }

    private void OnSpreadAcrossVersionTypes(object? parameter)
    {
      var pav = parameter as ProjectAndVersion;
      var project = pav?.Project ?? parameter as Project;
      var version = pav?.Version ?? parameter as SolutionVersionHandler.Model.Version;

      if (project == null) throw new ApplicationException("Unexpected null.");
      if (version == null) throw new ApplicationException("Unexpected null.");

      project.AssemblyVersion = version;
      project.Version = version;
      project.FileVersion = version;
      project.PackageVersion = version;
    }

    private void OnRemoveVersion(object? parameter)
    {
      var pav = parameter as ProjectAndVersion;
      var project = pav?.Project ?? parameter as Project;
      var version = pav?.Version ?? parameter as SolutionVersionHandler.Model.Version;

      if (project == null) throw new ApplicationException("Unexpected null.");
      if (version == null) throw new ApplicationException("Unexpected null.");

      if (project.AssemblyVersion == version)
        project.AssemblyVersion = null;
      else if (project.FileVersion == version)
        project.FileVersion = null;
      else if (project.PackageVersion == version)
        project.PackageVersion = null;
      else if (project.Version == version)
        project.Version = null;
      else
        throw new ApplicationException("Unknown version of the project.");
    }

    // Simple RelayCommand implementation
    internal class RelayCommand : ICommand
    {
      private readonly Action<object?> _execute;
      private readonly Predicate<object?>? _canExecute;

      public RelayCommand(Action<object?> execute, Predicate<object?>? canExecute = null)
      {
        _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        _canExecute = canExecute;
      }

      public bool CanExecute(object? parameter) => _canExecute?.Invoke(parameter) ?? true;

      public void Execute(object? parameter) => _execute(parameter);

      public event EventHandler? CanExecuteChanged
      {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
      }
    }
  }
}
