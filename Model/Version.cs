using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolutionVersionHandler.Model
{
  public class Version : ESystem.Miscelaneous.NotifyPropertyChanged
  {
    public int Major { get => base.GetProperty<int>(nameof(Major));
      set { base.UpdateProperty(nameof(Major), value); }
    }
    public int Minor { get => base.GetProperty<int>(nameof(Minor)); set => base.UpdateProperty(nameof(Minor), value); }
    public int Build { get => base.GetProperty<int>(nameof(Build)); set => base.UpdateProperty(nameof(Build), value); }
    public int Revision { get => base.GetProperty<int>(nameof(Revision)); set => base.UpdateProperty(nameof(Revision), value); }
    public string ShortVersion
    {
      get
      {
        if (Build == 0 && Revision == 0)
          return $"{Major}.{Minor}";
        else if (Revision == 0)
          return $"{Major}.{Minor}.{Build}";
        else
          return $"{Major}.{Minor}.{Build}.{Revision}";
      }
    }

    public string FullVersion => $"{Major}.{Minor}.{Build}.{Revision}";


  }
}
