using LiveCharts;
using LiveCharts.Wpf;
using System.Data.SqlClient;
using System.Windows;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace PFM
{
    /// <summary>
    /// ViewModel which handles all ViewModels of the application
    /// </summary>
    class MainViewModel : BaseViewModel
    {
        #region ViewModels

        public CategoryChartViewModel CategoryChart { get; set; }
        public ChartesianChartViewModel ColumnChart { get; set; }
        public ChartesianChartViewModel LineChart { get; set; }
        public InventoryViewModel DBInventory { get; set; }

        #endregion

        public PFMDBEntities entities { get; set; }
        public SqlConnection Con { get; set; }

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public MainViewModel()
        {
            Con = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=PFMDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            // Create new instance of InventoryViewModel
            DBInventory = new InventoryViewModel(this);

            //// Set up datas for pie chart
            //var pieChartQuery =
            //    from item in this.DBInventory.InventoryRecords
            //    where item.Type == "Kiadás" && item.Date.Month == DateTime.Now.AddMonths(-1).Month
            //    group item by item.Category into l
            //    select new { Category = l.Key, Total = l.Sum(records => records.Sum) };

            //SeriesCollection sc = new SeriesCollection();
            //foreach (var line in pieChartQuery)
            //{
            //    PieSeries ps = new PieSeries
            //    {
            //        Title = line.Category,
            //        Values = new ChartValues<int> { line.Total },
            //        DataLabels = true,
            //        LabelPoint = chartPoint =>
            //                            string.Format("{0:C0}",chartPoint.Y)
            //    };
            //    sc.Add(ps);
                
            //}
            
            // Create new instance of CategoryChartViewModel
            CategoryChart = new CategoryChartViewModel();
            SetPieChart();

            //Incomes vs. Expenditures

            //var incomesMonthlyQuery =
            //    from item in DBInventory.InventoryRecords
            //    where item.Type == "Bevétel" && item.Date.Month > DateTime.Now.AddMonths(-6).Month
            //    group item by item.Date.Month into l
            //    //orderby l.Key ascending
            //    select new { Month = l.Key, TotalIncome = l.Sum(records => records.Sum)};

            //incomesMonthlyQuery = incomesMonthlyQuery.Reverse();

            //var expendituresMonthlyQuery =
            //    from item in DBInventory.InventoryRecords
            //    where item.Type == "Kiadás" && item.Date.Month > DateTime.Now.AddMonths(-6).Month
            //    group item by item.Date.Month into l
            //    //orderby l.Key ascending
            //    select new { Month = l.Key, TotalExpenditure = l.Sum(records => records.Sum) };

            //expendituresMonthlyQuery = expendituresMonthlyQuery.Reverse();

            //SeriesCollection scIncomesExpenditures = new SeriesCollection();
            //ColumnSeries csIncomes = new ColumnSeries();
            //ColumnSeries csExpenditures = new ColumnSeries();
            //ChartValues<int> cvIncomes = new ChartValues<int>();
            //ChartValues<int> cvExpenditures = new ChartValues<int>();
            //foreach (var line in incomesMonthlyQuery)
            //{
            //    cvIncomes.Add(line.TotalIncome);
            //}
            //foreach (var line in expendituresMonthlyQuery)
            //{
            //    cvExpenditures.Add(line.TotalExpenditure);
            //}
            
            // Create a new instance of ChartesianChartViewModel
            ColumnChart = new ChartesianChartViewModel();
            SetColumnChart();

            //LineChart
            //var negativeSumQuery =
            //    from item in this.DBInventory.InventoryRecords
            //    where item.Type == "Kiadás"
            //    select new { Id = item.Id, Date= item.Date, Sum = (item.Sum * -1), Type = item.Type, Category = item.Category, Comment = item.Comment};

            //var positiveSumQuery =
            //    from item in this.DBInventory.InventoryRecords
            //    where item.Type == "Bevétel"
            //    select new { Id = item.Id, Date = item.Date, Sum = item.Sum, Type = item.Type, Category = item.Category, Comment = item.Comment };
            
            //var resultSumQuery = (negativeSumQuery.Concat(positiveSumQuery)).OrderBy(x => x.Date);
            //int cumSum = 0;
            //int actualBalance;

            //int[] b = resultSumQuery.Select(x => (cumSum += x.Sum)).ToArray();
            //if (b.Length == 0)
            //    actualBalance = 0;
            //else
            //    actualBalance = b.Last();
            //ChartValues<int> cv = new ChartValues<int>();
            //cv.AddRange(b);
            //string[] stringArray = new string[resultSumQuery.Count()];

            LineChart = new ChartesianChartViewModel();
            SetLineChart();


        }

        #endregion

        #region Methods

        public void SetPieChart()
        {
            Con.Open();
            entities = new PFMDBEntities();
            DateTime firstDayOfLastMonth = new DateTime(DateTime.Today.Year, DateTime.Today.AddMonths(-1).Month, 1);
            DateTime lastDayOfLastMonth = DateTime.Today.AddDays(-(DateTime.Today.Day));
            // Set up datas for pie chart
            var pieChartQuery =
                from item in entities.Inventory
                where item.Type == "Kiadás" && item.Date <= lastDayOfLastMonth && item.Date >= firstDayOfLastMonth
                group item by item.Category into l
                select new { Category = l.Key, Total = -l.Sum(records => records.Sum) };

            SeriesCollection sc = new SeriesCollection();
            foreach (var line in pieChartQuery)
            {
                PieSeries ps = new PieSeries
                {
                    Title = line.Category,
                    Values = new ChartValues<int> { line.Total },
                    DataLabels = true,
                    LabelPoint = chartPoint =>
                                        string.Format("{0:C0}", chartPoint.Y)
                };
                sc.Add(ps);

            }
            CategoryChart.PieSeries = sc;
            Con.Close();
        }

        public void SetColumnChart()
        {
            //Incomes vs. Expenditures

            //var incomesMonthlyQuery =
            //    from item in DBInventory.InventoryRecords
            //    where item.Type == "Bevétel" && item.Date.Month > DateTime.Now.AddMonths(-6).Month
            //    group item by item.Date.Month into l
            //    //orderby l.Key ascending
            //    select new { Month = l.Key, TotalIncome = l.Sum(records => records.Sum), Outgo = l.Where(rec => rec.Type == "Kiadás").Sum(r => r.)};

            //incomesMonthlyQuery = incomesMonthlyQuery.Reverse();

            //var expendituresMonthlyQuery =
            //    from item in DBInventory.InventoryRecords
            //    where item.Type == "Kiadás" && item.Date.Month > DateTime.Now.AddMonths(-6).Month
            //    group item by item.Date.Month into l
            //    //orderby l.Key ascending
            //    select new { Month = l.Key, TotalExpenditure = l.Sum(records => records.Sum) };

            //expendituresMonthlyQuery = expendituresMonthlyQuery.Reverse();
            DateTime firstDayOfFirstMonth = new DateTime(DateTime.Today.Year, DateTime.Today.AddMonths(-3).Month, 1);
            DateTime lastDayOfLastMonth = DateTime.Today.AddDays(-(DateTime.Today.Day));


            Con.Open();
            entities = new PFMDBEntities();

            var allItemMonthly =
                from item in entities.Inventory
                where item.Date >= firstDayOfFirstMonth && item.Date <= lastDayOfLastMonth orderby item.Date descending
                group item by item.Date.Month into l
                select new { Month = l.Key, TotalIncome = l.Where(records => records.Type == "Bevétel").Sum(records => records.Sum), TotalExpenditure = -l.Where(records => records.Type == "Kiadás").Sum(records => records.Sum) };

            
            ChartValues < int > cvIncomes = new ChartValues<int>();
            ChartValues<int> cvExpenditures = new ChartValues<int>();
            string[] labels = new string[allItemMonthly.Count()];
            //foreach (var line in incomesMonthlyQuery)
            //{
            //    cvIncomes.Add(line.TotalIncome);
            //}
            //foreach (var line in expendituresMonthlyQuery)
            //{
            //    cvExpenditures.Add(line.TotalExpenditure);
            //}
            int i = 0;
            foreach (var line in allItemMonthly)
            {
                cvIncomes.Add(line.TotalIncome);
                cvExpenditures.Add(line.TotalExpenditure);
                labels[i] = DateTimeFormatInfo.CurrentInfo.GetMonthName(line.Month);
                i++;
            }
            ColumnChart.SeriesCollection = new SeriesCollection
                {
                    new ColumnSeries
                    {
                        Title = "Bevétel",
                        Values = cvIncomes
                    },
                    new ColumnSeries
                    {
                        Title = "Kiadás",
                        Values = cvExpenditures
                    }
                };
            ColumnChart.Labels = labels;
                //new[] { DateTime.Now.AddMonths(-5).ToString("yyyy.MMMM"),
                //    DateTime.Now.AddMonths(-4).ToString("yyyy.MMMM"),
                //    DateTime.Now.AddMonths(-3).ToString("yyyy.MMMM"),
                //    DateTime.Now.AddMonths(-2).ToString("yyyy.MMMM"),
                //    DateTime.Now.AddMonths(-1).ToString("yyyy.MMMM"),
                //    DateTime.Now.ToString("yyyy.MMMM")};
            ColumnChart.Formatter = value => value.ToString("C0");
            Con.Close();
        }

        public void SetLineChart()
        {
            DateTime firstDayOfFirstMonth = new DateTime(DateTime.Today.Year, DateTime.Today.AddMonths(-3).Month, 1);
            DateTime lastDayOfLastMonth = DateTime.Today.AddDays(-(DateTime.Today.Day));

            Con.Open();
            entities = new PFMDBEntities();
            var getSumQuery =
                from item in entities.Inventory
                where item.Date >= firstDayOfFirstMonth && item.Date <= lastDayOfLastMonth orderby item.Date
                select new { Id = item.Id, Date = item.Date, Sum = item.Sum, Type = item.Type, Category = item.Category, Comment = item.Comment };

            //var positiveSumQuery =
            //    from item in entities.Inventory
            //    where item.Type == "Bevétel" && item.Date >= firstDayOfFirstMonth && item.Date <= lastDayOfLastMonth
            //    select new { Id = item.Id, Date = item.Date, Sum = item.Sum, Type = item.Type, Category = item.Category, Comment = item.Comment };

            var resultSumQuery = getSumQuery.ToList();
            int cumSum = 0;
            int actualBalance;

            int[] b = resultSumQuery.Select(x => (cumSum += x.Sum)).ToArray();
            if (b.Length == 0)
                actualBalance = 0;
            else
                actualBalance = b.Last();
            ChartValues<int> cv = new ChartValues<int>();
            cv.AddRange(b);
            string[] stringArray = new string[resultSumQuery.Count()];

            LineChart.SeriesCollection = new SeriesCollection()
                {
                    new LineSeries
                    {
                        Title = "Egyenleg",
                        Values = cv
                    }
                };
            LineChart.Labels = stringArray;
            LineChart.Formatter = value => value.ToString("C0");
            LineChart.ActualBalance = actualBalance.ToString("C0");
            Con.Close();
        }

        public void UpdateCharts()
        {
            SetPieChart();
            SetColumnChart();
            SetLineChart();
        }

        #endregion

    }
}
