using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;

namespace Readify.Converters
{
    public abstract class BaseMultiValueConverter<T> : MarkupExtension, IMultiValueConverter
        where T : class, new()
    {
        /// <summary>
        /// Хранит статичный экземпляр преобразователя
        /// </summary>
        private static T? _converter = null;

        /// <summary>
        /// Предоставляет экземпляр преобразователя
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        public override object? ProvideValue(IServiceProvider serviceProvider)
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
        /// <exception cref="NotImplementedException"></exception>
        public abstract object Convert(object[] values, Type targetType, object parameter, CultureInfo culture);

        /// <summary>
        /// Метод конвертирует обратно сконвертированный тип в исходный
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public virtual object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
