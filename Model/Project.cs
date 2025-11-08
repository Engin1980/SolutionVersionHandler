using ESystem.Miscelaneous;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolutionVersionHandler.Model
{
  public class Project : NotifyPropertyChanged
  {
    public string Name { get => base.GetProperty<string>(nameof(Name))!; set => base.UpdateProperty(nameof(Name), value); }
    public string FilePath { get => base.GetProperty<string>(nameof(FilePath))!; set => base.UpdateProperty(nameof(FilePath), value); }
    public Version? Version { get => base.GetProperty<Version>(nameof(Version))!; set => base.UpdateProperty(nameof(Version), value); }
    public Version? PackageVersion { get => base.GetProperty<Version>(nameof(PackageVersion))!; set => base.UpdateProperty(nameof(PackageVersion), value); }
    public Version? AssemblyVersion { get => base.GetProperty<Version>(nameof(AssemblyVersion))!; set => base.UpdateProperty(nameof(AssemblyVersion), value); }
    public Version? FileVersion { get => base.GetProperty<Version>(nameof(FileVersion))!; set => base.UpdateProperty(nameof(FileVersion), value); }
    public string? VersionPrefix { get => base.GetProperty<string?>(nameof(VersionPrefix)); set => base.UpdateProperty(nameof(VersionPrefix), value); }
    public string? VersionSuffix { get => base.GetProperty<string?>(nameof(VersionSuffix)); set => base.UpdateProperty(nameof(VersionSuffix), value); }
  }
}
