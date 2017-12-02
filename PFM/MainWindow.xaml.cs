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
        #region Public variables
        // Prepare connection to database
        SqlConnection con = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=PFMDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        
        #endregion

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
            dpDate.Text = DateTime.Now.ToString();
            DataContext = new MainViewModel(con);
        }

        #endregion
        
        #region Event handlers

        /// <summary>
        /// Handling Click event on pie chart
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="chartpoint"></param>

        private void Chart_OnDataClick(object sender, ChartPoint chartpoint)
        {
            var chart = (LiveCharts.Wpf.PieChart)chartpoint.ChartView;

            //clear selected slice.
            foreach (PieSeries series in chart.Series)
                series.PushOut = 0;

            var selectedSeries = (PieSeries)chartpoint.SeriesView;
            selectedSeries.PushOut = 8;
        }

        /// <summary>
        /// Add new item to database and ViewModel list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            // Insert new item to database
            con.Open();

            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "INSERT INTO dbo.Inventory (Type, Category, Sum, Date, Comment)" +
                              "VALUES(@type, @category, @sum, @date, @comment)";

            cmd.Parameters.AddWithValue("@type", ddType.Text);
            cmd.Parameters.AddWithValue("@category", ddCategory.Text);
            cmd.Parameters.AddWithValue("@sum", Convert.ToInt32(tbSum.Text));
            cmd.Parameters.AddWithValue("@date", Convert.ToDateTime(dpDate.Text));
            cmd.Parameters.AddWithValue("@comment", tbComment.Text.ToString());

            cmd.ExecuteNonQuery();

            con.Close();
            
            // Insert new item to ViewModel list
            MainViewModel vm = (MainViewModel)this.DataContext;
            vm.DBInventory.InventoryRecords.Add(new Inventory
            {
                Type = ddType.Text,
                Category = ddCategory.Text,
                Sum = Convert.ToInt32(tbSum.Text),
                Date = Convert.ToDateTime(dpDate.Text),
                Comment = tbComment.Text
            });
            // Sort list by date in descending order
            vm.DBInventory.InventoryRecords = vm.DBInventory.InventoryRecords.OrderByDescending(record => record.Date).ToList();

            // Bind ItemsSource to the updated list
            dgInventory.ItemsSource = null;
            dgInventory.ItemsSource = vm.DBInventory.InventoryRecords;

            // Clear the input fields
            vm.ItemType.SelectedItemType = vm.ItemType.Types[0]; ;
            tbSum.Text = "";
            dpDate.Text = DateTime.Now.ToString();
            tbComment.Text = "";
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
