using System;
using System.Globalization;
using System.Windows.Data;
using SolutionVersionHandler.Model;

namespace SolutionVersionHandler
{
  // MultiValueConverter that packages Project and Version into a Tuple<Project, Version>
  public class ProjectVersionMultiConverter : IMultiValueConverter
  {
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
      if (values == null || values.Length < 2)
        return null!;

      var project = values[0] as Project;
      var version = values[1] as SolutionVersionHandler.Model.Version;
      return new ProjectAndVersion(project, version);
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
      throw new NotSupportedException();
    }
  }
}
