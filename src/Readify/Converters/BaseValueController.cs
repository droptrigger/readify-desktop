using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace Readify.Converters
{
    /// <summary>
    /// Базовый класс для реализации конвертаторов
    /// </summary>
    /// <typeparam name="T">Является конкретным конвертатором, имеющим пустой конструтор</typeparam>
    public abstract class BaseValueController<T> : MarkupExtension, IValueConverter
        where T : class, new()
    {
        private static T? _converter = null;

        /// <summary>
        /// Представляет экземпляр преобразователя
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return _converter ??= new T();
        }

        /// <summary>
        /// Метод конвертирует один тип в другой
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public abstract object? Convert(object value, Type targetType, object parameter, CultureInfo culture);

        /// <summary>
        /// Метод конвертирует обратно сконвертированный тип в исходный
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public virtual object? ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
