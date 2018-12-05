using System;
using System.Globalization;
using PFM.ViewModels;
using PFM.Pages;

namespace PFM.ValueConverters
{
    public class PageConverter : BaseValueConverter<PageConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((PageList)value)
            {
                case PageList.Login:
                    return new LoginPage();
                case PageList.SignUp:
                    return new SignUpPage();
                default:
                    return null;
            }

        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
