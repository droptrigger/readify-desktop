using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace Readify.Converters
{
    /// <summary>
    /// Конвертатор из изображения в массив <see cref="byte[]"/>.
    /// Наследование от <see cref="IValueConverter"/>
    /// </summary>
    public class ImageConverter : IValueConverter
    {
        /// <summary>
        /// Метод конвертации
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is byte[] imageBytes)
            {
                using var ms = new MemoryStream(imageBytes);
                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                bitmap.StreamSource = ms;
                bitmap.EndInit();
                return bitmap;
            }

            // Пустое изображение
            return new BitmapImage(new Uri("pack://application:,,,/Readify;component/Images/default.png"));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
