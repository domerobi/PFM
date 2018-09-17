using System;
using System.Globalization;

namespace PFM
{
    public class PageConverter : BaseValueConverter<PageConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((Pages)value)
            {
                case Pages.Login:
                    return new LoginPage();
                case Pages.SignUp:
                    return new SignUpPage();
                case Pages.Reports:
                    return new ReportPage();
                case Pages.NewAccount:
                    return new NewAccountPage();
                case Pages.Properties:
                    return new PropertiesPage();
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
