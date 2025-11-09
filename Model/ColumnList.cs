using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolutionVersionHandler.Model
{
  public class ColumnList : BindingList<Column>
  {
    public const string VERSION_COLUMN_TITLE = "Version";
    public const string FILE_VERSION_COLUMN_TITLE = "FileVersion";
    public const string ASSEMBLY_VERSION_COLUMN_TITLE = "AssemblyVersion";
    public const string PACKAGE_VERSION_COLUMN_TITLE = "PackageVersion";
    public const string PROJECT_COLUMN_TITLE = "Project";
    public const string PROJECT_PATH_COLUMN_TITLE = "Project Path";

    public Column this[string title]
    {
      get => this.First(q => q.Title == title);
    }

    public void Create(string title, bool isVisible = true)
    {
      Column c = new Column()
      {
        IsChecked = true,
        IsVisible = isVisible,
        Title = title
      };
      this.Add(c);
    }

    public Column VersionColumn => this[VERSION_COLUMN_TITLE];
    public Column FileVersionColumn => this[FILE_VERSION_COLUMN_TITLE];
    public Column AssemblyVersionColumn => this[ASSEMBLY_VERSION_COLUMN_TITLE];
    public Column PackageVersionColumn => this[PACKAGE_VERSION_COLUMN_TITLE];
    public Column ProjectColumn => this[PROJECT_COLUMN_TITLE];
    public Column ProjectPathColumn => this[PROJECT_PATH_COLUMN_TITLE];
  }
}
