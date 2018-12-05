using LiveCharts;
using LiveCharts.Wpf;
using System.Data.Entity;
using System.Windows;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using PFM.Models;
using System.Collections.ObjectModel;

namespace PFM.ViewModels
{
    class ReportViewModel : BaseViewModel
    {
        #region ViewModels

        public ObservableCollection<IChartModel> ReportViewModels { get; set; }
        public IChartModel CurrentViewModel { get; set; }
        public MainViewModel MainViewModel { get; set; }

        public CategoryChartViewModel ExpendCategories { get; set; }
        public CategoryChartViewModel IncomeCategories { get; set; }
        public ChartesianChartViewModel StackedSixMonths { get; set; }
        public BalanceViewModel BalanceViewModel { get; set; }

        #endregion

        public ReportViewModel(MainViewModel mainViewModel)
        {
            Name = "Kimutatások";

            MainViewModel = mainViewModel;

            ExpendCategories = new CategoryChartViewModel();
            ExpendCategories.Name = "Kiadások";
            ExpendCategories.Title = ExpendCategories.Name;

            IncomeCategories = new CategoryChartViewModel();
            IncomeCategories.Name = "Bevételek";
            IncomeCategories.Title = IncomeCategories.Name;

            StackedSixMonths = new ChartesianChartViewModel();
            StackedSixMonths.Name = "Féléves kiadások";
            StackedSixMonths.Title = StackedSixMonths.Name;
            StackedSixMonths.Formatter = value => value.ToString("C0");
            StackedSixMonths.XTitle = "Hónapok";
            StackedSixMonths.YTitle = "Összeg";

            BalanceViewModel = new BalanceViewModel(MainViewModel);

            // Initialize all of the report categories to be displayed
            ReportViewModels = new ObservableCollection<IChartModel>
            {
                ExpendCategories,
                IncomeCategories,
                StackedSixMonths,
                BalanceViewModel
            };
            CurrentViewModel = ReportViewModels[0];

            SetCategoryPieCharts();
            SetStackedColumnChart();
            BalanceViewModel.SetLineChart();
        }

        public void SetCategoryPieCharts()
        {
            using (var db = new DataModel())
            {
                // let's clear the series first
                IncomeCategories.Series = new List<Series>();
                ExpendCategories.Series = new List<Series>();
                IncomeCategories.PieSeries = new SeriesCollection();
                ExpendCategories.PieSeries = new SeriesCollection();

                // Set the time interval for the chart
                DateTime lastMonth = DateTime.Today.AddMonths(-1);
                DateTime firstDayOfLastMonth = new DateTime(lastMonth.Year, lastMonth.Month, 1);
                DateTime lastDayOfLastMonth = DateTime.Today.AddDays(-(DateTime.Today.Day));

                // Get the sum of all categories of the last month
                var groupbyCategories = db.Transactions
                                            .Include(t => t.Categories.CategoryDirections)
                                            .Where(t => t.TransactionDate >= firstDayOfLastMonth &&
                                                        t.TransactionDate <= lastDayOfLastMonth &&
                                                        t.AccountID == MainViewModel.CurrentAccount.AccountID)
                                            .GroupBy(t => t.Categories.CategoryName)
                                            .Select(t => new
                                            {
                                                Category = t.Key,
                                                CategoryDirection = t.FirstOrDefault().Categories.CategoryDirections.DirectionName,
                                                Amount = t.Sum(c => c.Amount)
                                            }).ToList();


                foreach (var cat in groupbyCategories)
                {
                    PieSeries ps = new PieSeries
                    {
                        Title = cat.Category,
                        Values = new ChartValues<int> { (int)cat.Amount },
                        DataLabels = true,
                        LabelPoint = chartPoint =>
                                            string.Format("{0:C0}", chartPoint.Y)
                    };
                    if (cat.CategoryDirection == "Kiadás")
                        ExpendCategories.Series.Add(ps);
                    if (cat.CategoryDirection == "Bevétel")
                        IncomeCategories.Series.Add(ps);
                }
                ExpendCategories.PieSeries.AddRange(ExpendCategories.Series);
                IncomeCategories.PieSeries.AddRange(IncomeCategories.Series);
            }
        }

        public void SetStackedColumnChart()
        {
            // let's clear the series first
            StackedSixMonths.Series = new List<Series>();
            StackedSixMonths.SeriesCollection = new SeriesCollection();

            // set the default filter dates
            DateTime sixBefore = DateTime.Today.AddMonths(-6);
            DateTime firstDay = new DateTime(sixBefore.Year, sixBefore.Month, 1);
            DateTime lastDay = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddDays(-1);

            List<int> months = new List<int>();
            for (var i = new DateTime(firstDay.Year, firstDay.Month, firstDay.Day); i < lastDay; i = i.AddMonths(1))
            {
                months.Add(i.Month);
            }

            using (var db = new DataModel())
            {
                var allTransactions = db.Transactions
                                        .Include(t => t.Categories.CategoryDirections)
                                        .Where(t => t.AccountID == MainViewModel.CurrentAccount.AccountID &&
                                                    t.TransactionDate >= firstDay &&
                                                    t.TransactionDate <= lastDay &&
                                                    t.Categories.CategoryDirections.DirectionName == "Kiadás")
                                        .GroupBy(t => new { t.TransactionDate.Year, t.TransactionDate.Month, t.Categories.CategoryName })
                                        .Select(t => new
                                        {
                                            Year = t.Key.Year,
                                            Month = t.Key.Month,
                                            Category = t.Key.CategoryName,
                                            Amount = t.Sum(s => s.Amount)
                                        })
                                        .OrderBy(t => new { t.Year, t.Month, t.Category })
                                        .ToList();
                var categoryList = allTransactions.Select(t => t.Category).Distinct().ToList();
                foreach (var category in categoryList)
                {
                    ChartValues<double> values = new ChartValues<double>();
                    var currentCategory = allTransactions.Where(t => t.Category == category).ToList();
                    foreach (var month in months)
                    {
                        var currentAmount = currentCategory.FirstOrDefault(c => c.Month == month);
                        if (currentAmount != null)
                            values.Add((double)currentAmount.Amount);
                        else
                            values.Add(0);
                    }
                    StackedSixMonths.Series
                    .Add(new StackedColumnSeries
                    {
                        Title = category,
                        Values = values,
                        DataLabels = false
                    });
                }

                StackedSixMonths.SeriesCollection.AddRange(StackedSixMonths.Series);

                StackedSixMonths.Labels = new string[6];
                foreach (var month in months)
                {
                    StackedSixMonths.Labels[months.IndexOf(month)] = DateTimeFormatInfo.CurrentInfo.GetMonthName(month);
                }
            }
        }

        public void RefreshCharts()
        {
            SetCategoryPieCharts();
            SetStackedColumnChart();
        }
    }
}
