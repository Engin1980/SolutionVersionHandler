using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace SolutionVersionHandler
{
  public class VersionParseableVisibilityConverter : IMultiValueConverter
  {
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
      if (values == null || values.Length < 2)
        return Visibility.Collapsed;

      var versionObj = values[0];
      var unparseable = values[1];

      // Visible only when version exists and unparseable is null
      if (versionObj != null && unparseable == null)
        return Visibility.Visible;

      return Visibility.Collapsed;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
      throw new NotSupportedException();
    }
  }
}
