using ESystem.Miscelaneous;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SolutionVersionHandler.Model
{
  public class Column : NotifyPropertyChanged
  {
    public string Title
    {
      get => base.GetProperty<string>(nameof(Title))!;
      set => base.UpdateProperty(nameof(Title), value);
    }
    public bool IsVisible
    {
      get => base.GetProperty<bool>(nameof(IsVisible))!;
      set
      {
        base.UpdateProperty(nameof(IsVisible), value);
        base.UpdateProperty(nameof(Visibility), value ? Visibility.Visible : Visibility.Collapsed);
      }
    }

    public bool IsChecked
    {
      get => base.GetProperty<bool>(nameof(IsChecked));
      set => base.UpdateProperty(nameof(IsChecked), value);
    }

    public Visibility Visibility
    {
      get => base.GetProperty<Visibility>(nameof(Visibility));
    }
  }
}
