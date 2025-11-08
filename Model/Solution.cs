using ESystem.Miscelaneous;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolutionVersionHandler.Model
{
  internal class Solution : NotifyPropertyChanged
  {
    public string FilePath { get => base.GetProperty<string>(nameof(FilePath))!; set => base.UpdateProperty(nameof(FilePath), value); }
    public string Name { get => base.GetProperty<string>(nameof(Name))!; set => base.UpdateProperty(nameof(Name), value); }
    public List<Project> Projects { get => base.GetProperty<List<Project>>(nameof(Projects))!; set => base.UpdateProperty(nameof(Projects), value); }
  }
}
