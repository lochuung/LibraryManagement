using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;

namespace LibraryManagement.Utils.Converter
{
    public class ImageToSource : IValueConverter
    {
        public object Convert(object value, Type targetType, object paramater, CultureInfo culture)
        {
            if (value == null)
                return 0;
            string fileName = (string)value;

            string destPath = Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory()));
            string destinationFile = Path.Combine($"{destPath}\\BookImageCover", fileName);
            return destinationFile;
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("MethodToValueConverter can only be used for one way conversion.");
        }
    }
}