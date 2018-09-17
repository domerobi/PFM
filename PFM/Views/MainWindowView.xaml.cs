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

namespace PFM
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindowView : Window
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public MainWindowView(int userid)
        {
            InitializeComponent();
            DataContext = new MainViewModel(userid);
        }

        #endregion

        private void MainFrame_ContentRendered(object sender, EventArgs e)
        {
            MainFrame.NavigationUIVisibility = System.Windows.Navigation.NavigationUIVisibility.Hidden;
        }

        private void Logout(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
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
            return (width <= 1) ? 0 : (width - 2.1);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter,
            System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }

    #endregion

}
