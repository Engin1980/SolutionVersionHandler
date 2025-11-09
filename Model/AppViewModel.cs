using ESystem.Miscelaneous;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SolutionVersionHandler.Model
{
  public class AppViewModel : NotifyPropertyChanged
  {
    public static AppViewModel Instance { get; private set; }

    static AppViewModel()
    {
      AppViewModel.Instance = new()
      {
        Columns = [],
        Projects = []
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
      set => UpdateProperty(nameof(SolutionFile), value);
    }
    public ColumnList Columns
    {
      get => GetProperty<ColumnList>(nameof(Columns))!;
      set => UpdateProperty(nameof(Columns), value);
    }
  }
}
