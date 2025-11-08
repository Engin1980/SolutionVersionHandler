using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolutionVersionHandler.Model
{
  public class Version : ESystem.Miscelaneous.NotifyPropertyChanged
  {
    public int Major
    {
      get => base.GetProperty<int>(nameof(Major));
      set { base.UpdateProperty(nameof(Major), value); }
    }
    public int Minor { get => base.GetProperty<int>(nameof(Minor)); set => base.UpdateProperty(nameof(Minor), value); }
    public int Build { get => base.GetProperty<int>(nameof(Build)); set => base.UpdateProperty(nameof(Build), value); }
    public int Revision { get => base.GetProperty<int>(nameof(Revision)); set => base.UpdateProperty(nameof(Revision), value); }
    public string? Unparseable { get => base.GetProperty<string?>(nameof(Unparseable)); set => base.UpdateProperty(nameof(Unparseable), value); }
    public string ShortVersion
    {
      get
      {
        if (Unparseable != null)
          return Unparseable;
        if (Build == 0 && Revision == 0)
          return $"{Major}.{Minor}";
        else if (Revision == 0)
          return $"{Major}.{Minor}.{Build}";
        else
          return $"{Major}.{Minor}.{Build}.{Revision}";
      }
    }

    public string FullVersion => Unparseable != null ? Unparseable : $"{Major}.{Minor}.{Build}.{Revision}";

    internal static Version AsUnparseable(string tmp)
    {
      return new Version()
      {
        Unparseable = tmp
      };
    }

    internal Version Copy()
    {
      return new Version()
      {
        Unparseable = this.Unparseable,
        Major = this.Major,
        Minor = this.Minor,
        Build = this.Build,
        Revision = this.Revision
      };
    }
  }
}
