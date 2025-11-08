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
      var item = parameter as Project;
      if (item != null)
      {
        MessageBox.Show($"Spread across projects requested for {item.Name}");
      }
    }

    private void OnSpreadAcrossVersionTypes(object? parameter)
    {
      var item = parameter as Project;
      if (item != null)
      {
        MessageBox.Show($"Spread across version types requested for {item.Name}");
      }
    }

    private void OnRemoveVersion(object? parameter)
    {
      var item = parameter as Project;
      if (item != null)
      {
        MessageBox.Show($"Remove version requested for {item.Name}");
      }
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
