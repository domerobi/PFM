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

        SqlConnection con = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=PFMDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        public Func<double, string> Formatter { get; set; }
        public SeriesCollection SeriesCollectionBasic { get; set; }
        public SeriesCollection SeriesCollection { get; set; }
        public string[] LabelsBasic { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> YFormatter { get; set; }

        public Func<ChartPoint, string> PointLabel { get; set; }

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
            // Set initial data for DataGrid
            //BindDataGrid();

            //PointLabel = chartPoint =>
            //    string.Format("{0} ({1:P})", chartPoint.Y, chartPoint.Participation);

            //SeriesCollection = new SeriesCollection()
            //{
            //    new LineSeries
            //    {
            //        Title = "Series 1",
            //        Values = new ChartValues<double> { 4, 6, 5, 2 ,4 }
            //    },
            //    new LineSeries
            //    {
            //        Title = "Series 2",
            //        Values = new ChartValues<double> { 6, 7, 3, 4 ,6 },
            //        PointGeometry = null
            //    },
            //    new LineSeries
            //    {
            //        Title = "Series 3",
            //        Values = new ChartValues<double> { 4,2,7,2,7 },
            //        PointGeometry = DefaultGeometries.Square,
            //        PointGeometrySize = 15
            //    }
            //};

            //Labels = new[] { "Jan", "Feb", "Mar", "Apr", "May" };
            //YFormatter = value => value.ToString("C");

            ////modifying the series collection will animate and update the chart
            ///*SeriesCollection.Add(new LineSeries
            //{
            //    Title = "Series 4",
            //    Values = new ChartValues<double> { 5, 3, 2, 4 },
            //    LineSmoothness = 0, //0: straight lines, 1: really smooth lines
            //    PointGeometry = Geometry.Parse("m 25 70.36218 20 -28 -20 22 -8 -6 z"),
            //    PointGeometrySize = 50,
            //    PointForeground = Brushes.Gray
            //});*/

            ////modifying any series values will also animate and update the chart
            ////SeriesCollection[3].Values.Add(5d);



            ////BASIC plot
            //SeriesCollectionBasic = new SeriesCollection
            //{
            //    new ColumnSeries
            //    {
            //        Title = "2015",
            //        Values = new ChartValues<double> { 10, 50, 39, 50 }
            //    }
            //};

            ////adding series will update and animate the chart automatically
            //SeriesCollectionBasic.Add(new ColumnSeries
            //{
            //    Title = "2016",
            //    Values = new ChartValues<double> { 11, 56, 42 }
            //});

            ////also adding values updates and animates the chart automatically
            //SeriesCollectionBasic[1].Values.Add(48d);

            //LabelsBasic = new[] { "Maria", "Susan", "Charles", "Frida" };
            //Formatter = value => value.ToString("N");

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
        
        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
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
            //BindDataGrid();



        }

        #endregion


        /// <summary>
        /// Update DataGrid
        /// </summary>
        //private void BindDataGrid()
        //{
        //    con.Open();

        //    PFMDBEntities dataEntities = new PFMDBEntities();
        //    var query =
        //        from item in dataEntities.Inventory
        //        //where item.Category == "Utazás"
        //        orderby item.Date descending
        //        select new { item.Id, item.Date, item.Type, item.Category, item.Sum, item.Comment  };

        //    dgInventory.ItemsSource = query.ToList();


        //    //SqlCommand cmd = con.CreateCommand();
        //    //cmd.CommandType = CommandType.Text;
        //    //cmd.CommandText = "SELECT Id, Date, Type, Category, Sum, Comment FROM dbo.Inventory ORDER BY Date desc";
        //    //cmd.Connection = con;
        //    //SqlDataAdapter da = new SqlDataAdapter(cmd);
        //    //DataTable dt = new DataTable("Inventory");
        //    ////DataView view = dt.AsDataView();
        //    ////view.RowFilter = "Category == Utazás";
            
        //    //dgInventory.ItemsSource = dt.DefaultView;
        //    con.Close();

        //}
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
