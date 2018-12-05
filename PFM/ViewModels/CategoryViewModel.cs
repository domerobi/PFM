using System;
using System.Linq;
using System.Data.Entity;
using LiveCharts;
using LiveCharts.Wpf;
using PFM.Models;
using System.Globalization;
using System.Collections.Generic;

namespace PFM.ViewModels
{
    class CategoryViewModel : BaseViewModel
    {

        #region Public properties

        public MainViewModel MainViewModel { get; set; }

        #endregion

        #region Series of data

        public SeriesCollection IncomeCategories { get; set; }
        public SeriesCollection ExpendCategories { get; set; }

        public ChartesianChartViewModel StackedCategories { get; set; }


        #endregion

        public CategoryViewModel(MainViewModel mainViewModel)
        {
            Name = "Kategóriák";

            MainViewModel = mainViewModel;

            SetCategoryPieCharts();
            
        }

        public void SetCategoryPieCharts()
        {
            IncomeCategories = new SeriesCollection();
            ExpendCategories = new SeriesCollection();
            using (var db = new DataModel())
            {
                // Set the time interval for the chart
                DateTime lastMonth = DateTime.Today.AddMonths(-1);
                DateTime firstDayOfLastMonth = new DateTime(lastMonth.Year, lastMonth.Month, 1);
                DateTime lastDayOfLastMonth = DateTime.Today.AddDays(-(DateTime.Today.Day));

                // Get the sum of all categories of the last month
                var groupbyCategories = db.Transactions
                                            .Include(t => t.Categories.CategoryDirections)
                                            .Where(t => t.TransactionDate >= firstDayOfLastMonth && 
                                                        t.TransactionDate <= lastDayOfLastMonth  &&
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
                        ExpendCategories.Add(ps);
                    if (cat.CategoryDirection == "Bevétel")
                        IncomeCategories.Add(ps);
                }
            }
        }

        

    }
}
