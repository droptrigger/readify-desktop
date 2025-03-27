using System.Globalization;
using System.Windows.Data;
using System.Windows;

namespace Readify.Converters
{
    /// <summary>
    /// Конвертер видимости из <see cref="Boolean"/> <see cref="Visibility"/>.
    /// Наследование от <see cref="IValueConverter"/>
    /// </summary>
    public class BoolToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// Метод конвертации
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns><see cref="Visibility.Visible"/>, если True, иначе <see cref="Visibility.Collapsed"/></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
            (value is bool b && b) ? Visibility.Visible : Visibility.Collapsed;

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
