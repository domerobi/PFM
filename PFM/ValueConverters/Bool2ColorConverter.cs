using System;
using System.Globalization;
using System.Windows.Data;

namespace PFM.ValueConverters
{
    class Bool2ColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool original = (bool)value;
            return original ? "#F2E38A" : "#729CD4";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string original = (string)value;
            return original == "#F2E38A" ? true : false;
        }
    }
}
