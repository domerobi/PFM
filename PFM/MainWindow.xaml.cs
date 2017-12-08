using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using LiveCharts;
using LiveCharts.Wpf;
using System.Data.SqlClient;
using System.Data;
using System.Threading;
using System.Globalization;
using System.Windows.Markup;
using System.Linq;

namespace PFM
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public MainWindow()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("hu-HU");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("hu-HU");
            FrameworkElement.LanguageProperty.OverrideMetadata(typeof(FrameworkElement), new FrameworkPropertyMetadata(
                        XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));
            InitializeComponent();
            DataContext = new MainViewModel();
        }

        #endregion
    }

    #region Converter

    public class TabSizeConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            TabControl tabControl = values[0] as TabControl;
            double width = tabControl.ActualWidth / tabControl.Items.Count;
            //Subtract 1, otherwise we could overflow to two rows.
            return (width <= 1) ? 0 : (width - 1);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter,
            System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }

    #endregion

}
