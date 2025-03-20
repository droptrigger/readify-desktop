using System.Globalization;
using System.Windows;

namespace Readify.Converters
{
    /// <summary>
    /// Конвертирует <see cref="Boolean"/> в <see cref="Visibility"/>
    /// </summary>
    public class BooleanToVisibilityConverter : BaseValueController<BooleanToVisibilityConverter>
    {
        public override object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? Visibility.Visible : Visibility.Collapsed; 
        }
    }
}
