using LiveCharts;
using LiveCharts.Wpf;
using System.Data.SqlClient;

namespace PFM
{
    /// <summary>
    /// ViewModel which handles all ViewModels of the application
    /// </summary>
    class MainViewModel : BaseViewModel
    {
        #region ViewModels

        public ItemTypeViewModel ItemType { get; set; }
        public CategoryChartViewModel CategoryChart { get; set; }
        public ChartesianChartViewModel ColumnChart { get; set; }
        public ChartesianChartViewModel LineChart { get; set; }
        public InventoryViewModel DBInventory { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="con">Connection to the database</param>
        public MainViewModel(SqlConnection con)
        {
            this.ItemType = new ItemTypeViewModel();
            this.CategoryChart = new CategoryChartViewModel
            {
                PieSeries = new SeriesCollection
                {
                    new PieSeries
                    {
                        Title = "Maria",
                        Values = new ChartValues<double>{3},
                        DataLabels = true,
                        LabelPoint =  chartPoint =>
                                        string.Format("{0} ({1:P})", chartPoint.Y, chartPoint.Participation)
                    },
                    new PieSeries
                    {
                        Title = "Charles",
                        Values = new ChartValues<double>{4},
                        DataLabels = true,
                        LabelPoint =  chartPoint =>
                                        string.Format("{0} ({1:P})", chartPoint.Y, chartPoint.Participation)
                    },
                    new PieSeries
                    {
                        Title = "Frida",
                        Values = new ChartValues<double>{6},
                        DataLabels = true,
                        LabelPoint =  chartPoint =>
                                        string.Format("{0} ({1:P})", chartPoint.Y, chartPoint.Participation)
                    },
                    new PieSeries
                    {
                        Title = "Frederic",
                        Values = new ChartValues<double>{2},
                        DataLabels = true,
                        LabelPoint =  chartPoint =>
                                        string.Format("{0} ({1:P})", chartPoint.Y, chartPoint.Participation)
                    }
                }
            };
            this.ColumnChart = new ChartesianChartViewModel
            {
                SeriesCollection = new SeriesCollection
                {
                    new ColumnSeries
                    {
                        Title = "2015",
                        Values = new ChartValues<double> { 10, 50, 39, 50 }
                    },
                    new ColumnSeries
                    {
                        Title = "2016",
                        Values = new ChartValues<double> { 11, 56, 42, 46 }
                    }
                },
                Labels = new[] { "Maria", "Susan", "Charles", "Frida" },
                Formatter = value => value.ToString("N")
            };
            this.LineChart = new ChartesianChartViewModel
            {
                SeriesCollection = new SeriesCollection()
                {
                    new LineSeries
                    {
                        Title = "Series 1",
                        Values = new ChartValues<double> { 4, 6, 5, 2 ,4 }
                    },
                    new LineSeries
                    {
                        Title = "Series 2",
                        Values = new ChartValues<double> { 6, 7, 3, 4 ,6 },
                        PointGeometry = null
                    },
                    new LineSeries
                    {
                        Title = "Series 3",
                        Values = new ChartValues<double> { 4,2,7,2,7 },
                        PointGeometry = DefaultGeometries.Square,
                        PointGeometrySize = 15
                    }
                },
                Labels = new[] { "Jan", "Feb", "Mar", "Apr", "May" },
                Formatter = value => value.ToString("C")
            };
            this.DBInventory = new InventoryViewModel(con);
        }

        #endregion

    }
}
