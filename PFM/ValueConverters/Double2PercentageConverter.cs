using System;
using System.Globalization;
using System.Windows.Data;

namespace PFM.ValueConverters
{
    class Double2PercentageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double original = (double)value;
            return original * 100;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double original;
            Double.TryParse((string)value, out original);
            return original / 100;
        }
    }
}
