using LiveCharts;
using LiveCharts.Wpf;
using System.Data.SqlClient;
using System.Windows;
using System.Linq;
using System;
using System.Collections.Generic;

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

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public MainViewModel()
        {
            // Create new instance of InventoryViewModel
            this.DBInventory = new InventoryViewModel();

            // Set up datas for pie chart
            var pieChartQuery =
                from item in this.DBInventory.InventoryRecords
                where item.Type == "Kiadás" && item.Date.Month == DateTime.Now.AddMonths(-1).Month
                group item by item.Category into l
                select new { Category = l.Key, Total = l.Sum(records => records.Sum) };

            SeriesCollection sc = new SeriesCollection();
            foreach (var line in pieChartQuery)
            {
                PieSeries ps = new PieSeries
                {
                    Title = line.Category,
                    Values = new ChartValues<int> { line.Total },
                    DataLabels = true,
                    LabelPoint = chartPoint =>
                                        string.Format("{0:C0}",chartPoint.Y)
                };
                sc.Add(ps);
                
            }
            
            // Create new instance of CategoryChartViewModel
            this.CategoryChart = new CategoryChartViewModel
            {
                PieSeries = sc
            };

            //Incomes vs. Expenditures

            var incomesMonthlyQuery =
                from item in this.DBInventory.InventoryRecords
                where item.Type == "Bevétel" && item.Date.Month > DateTime.Now.AddMonths(-6).Month
                group item by item.Date.Month into l
                //orderby l.Key ascending
                select new { Month = l.Key, TotalIncome = l.Sum(records => records.Sum)};

            incomesMonthlyQuery = incomesMonthlyQuery.Reverse();

            var expendituresMonthlyQuery =
                from item in this.DBInventory.InventoryRecords
                where item.Type == "Kiadás" && item.Date.Month > DateTime.Now.AddMonths(-6).Month
                group item by item.Date.Month into l
                //orderby l.Key ascending
                select new { Month = l.Key, TotalExpenditure = l.Sum(records => records.Sum) };

            expendituresMonthlyQuery = expendituresMonthlyQuery.Reverse();

            SeriesCollection scIncomesExpenditures = new SeriesCollection();
            ColumnSeries csIncomes = new ColumnSeries();
            ColumnSeries csExpenditures = new ColumnSeries();
            ChartValues<int> cvIncomes = new ChartValues<int>();
            ChartValues<int> cvExpenditures = new ChartValues<int>();
            foreach (var line in incomesMonthlyQuery)
            {
                cvIncomes.Add(line.TotalIncome);
            }
            foreach (var line in expendituresMonthlyQuery)
            {
                cvExpenditures.Add(line.TotalExpenditure);
            }
            
            // Create a new instance of ChartesianChartViewModel
            this.ColumnChart = new ChartesianChartViewModel
            {
                SeriesCollection = new SeriesCollection
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
                },
                Labels = new[] { DateTime.Now.AddMonths(-5).ToString("yyyy.MMMM"),
                    DateTime.Now.AddMonths(-4).ToString("yyyy.MMMM"),
                    DateTime.Now.AddMonths(-3).ToString("yyyy.MMMM"),
                    DateTime.Now.AddMonths(-2).ToString("yyyy.MMMM"),
                    DateTime.Now.AddMonths(-1).ToString("yyyy.MMMM"),
                    DateTime.Now.ToString("yyyy.MMMM")},
                Formatter = value => value.ToString("C0")
            };

            //LineChart
            var negativeSumQuery =
                from item in this.DBInventory.InventoryRecords
                where item.Type == "Kiadás"
                select new { Id = item.Id, Date= item.Date, Sum = (item.Sum * -1), Type = item.Type, Category = item.Category, Comment = item.Comment};

            var positiveSumQuery =
                from item in this.DBInventory.InventoryRecords
                where item.Type == "Bevétel"
                select new { Id = item.Id, Date = item.Date, Sum = item.Sum, Type = item.Type, Category = item.Category, Comment = item.Comment };
            
            var resultSumQuery = (negativeSumQuery.Concat(positiveSumQuery)).OrderBy(x => x.Date);
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

            this.LineChart = new ChartesianChartViewModel
            {
                SeriesCollection = new SeriesCollection()
                {
                    new LineSeries
                    {
                        Title = "Egyenleg",
                        Values = cv
                    }
                },
                Labels = stringArray,
                Formatter = value => value.ToString("C0"),
                ActualBalance = actualBalance.ToString("C0")
            };



        }

        #endregion

    }
}
