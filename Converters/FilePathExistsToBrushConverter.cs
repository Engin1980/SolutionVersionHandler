using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media;

namespace SolutionVersionHandler
{
  public class FilePathExistsToBrushConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      var path = value as string;
      bool exists = !string.IsNullOrEmpty(path) && File.Exists(path);
      return exists ? Brushes.Black : Brushes.Red;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      throw new NotSupportedException();
    }
  }
}
