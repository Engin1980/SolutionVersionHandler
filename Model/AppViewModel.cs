using ESystem.Miscelaneous;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SolutionVersionHandler.Model;

public class AppViewModel : NotifyPropertyChanged
{
  public record RecentSolutionFile(string FilePath)
  {
    public string FileName => System.IO.Path.GetFileName(this.FilePath);
  }

  public static AppViewModel Instance { get; private set; }

  static AppViewModel()
  {
    AppViewModel.Instance = new()
    {
      Columns = [],
      Projects = [],
      RecentSolutionFiles = [],
      State = new()
    };
  }

  public BindingList<Project> Projects
  {
    get => GetProperty<BindingList<Project>>(nameof(Projects))!;
    set => UpdateProperty(nameof(Projects), value);
  }
  public string? SolutionFile
  {
    get => GetProperty<string>(nameof(SolutionFile));
    set
    {
      UpdateProperty(nameof(SolutionFile), value);
      State.IsSomeSolutionFileSet = true;
    }
  }
  public ColumnList Columns
  {
    get => GetProperty<ColumnList>(nameof(Columns))!;
    set => UpdateProperty(nameof(Columns), value);
  }

  public BindingList<RecentSolutionFile> RecentSolutionFiles
  {
    get => GetProperty<BindingList<RecentSolutionFile>>(nameof(RecentSolutionFiles))!;
    set => UpdateProperty(nameof(RecentSolutionFiles), value);
  }
  public State State
  {
    get => GetProperty<State>(nameof(State))!;
    private set => UpdateProperty(nameof(State), value);
  }
}

public class State : NotifyPropertyChanged
{
  public bool IsSomeSolutionFileSet
  {
    get => GetProperty<bool>(nameof(IsSomeSolutionFileSet));
    set
    {
      UpdateProperty(nameof(IsSomeSolutionFileSet), value);
      UpdateStates();
    }
  }
  public bool IsBusy
  {
    get => GetProperty<bool>(nameof(IsBusy));
    set
    {
      UpdateProperty(nameof(IsBusy), value);
      UpdateStates();
    }
  }

  private void UpdateStates()
  {
    IsOpenLikeButtonEnabled = !IsBusy;
    IsSaveLikeButtonEnabled = IsSomeSolutionFileSet && !IsBusy;
  }

  public bool IsOpenLikeButtonEnabled
  {
    get => GetProperty<bool>(nameof(IsOpenLikeButtonEnabled));
    private set => UpdateProperty(nameof(IsOpenLikeButtonEnabled), value);
  }

  public bool IsSaveLikeButtonEnabled
  {
    get => GetProperty<bool>(nameof(IsSaveLikeButtonEnabled));
    private set => UpdateProperty(nameof(IsSaveLikeButtonEnabled), value);
  }

}
