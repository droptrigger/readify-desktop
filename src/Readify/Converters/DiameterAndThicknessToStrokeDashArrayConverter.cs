using System.Globalization;
using System.Windows.Media;

namespace Readify.Converters
{
    /// <summary>
    /// Используется при MultiBinding конвертации в StrokeDashArray
    /// </summary>
    public class DiameterAndThicknessToStrokeDashArrayConverter : BaseMultiValueConverter<DiameterAndThicknessToStrokeDashArrayConverter>
    {
        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null ||
                values.Length < 0 ||
                !double.TryParse(values[0].ToString(), out double diameter) ||
                !double.TryParse(values[1].ToString(), out double thickness))
            {
                return new DoubleCollection(new[] { 0.0 });
            }

            double circumference = Math.PI * diameter;

            double lineLenght = circumference * 0.75;
            double gapLength = circumference - lineLenght;

            return new DoubleCollection(new[] { lineLenght / thickness, gapLength / thickness});
        }
    }
}
