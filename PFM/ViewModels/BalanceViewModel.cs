using System;
using System.Linq;
using System.Data.Entity;

using PFM.Models;
using LiveCharts;
using LiveCharts.Wpf;

namespace PFM.ViewModels
{
    class BalanceViewModel : BaseViewModel, IChartModel
    {

        #region Public properties

        public ChartesianChartViewModel LineChart { get; set; }
        public MainViewModel MainViewModel { get; private set; }

        public bool HasCalculation
        {
            get
            {
                return false;
            }
        }

        #endregion

        public BalanceViewModel(MainViewModel mainViewModel)
        {
            Name = "Egyenleg";

            MainViewModel = mainViewModel;
        }


        public void SetLineChart()
        {
            DateTime firstDayOfFirstMonth = DateTime.Today.AddDays(-(DateTime.Today.Day - 1)).AddMonths(-6);
            DateTime today = DateTime.Today;

            LineChart = new ChartesianChartViewModel();
            LineChart.Title = "Egyenleg változása";
            LineChart.Formatter = value => value.ToString("C0");

            using (var db = new DataModel())
            {
                var allTransaction = db.Transactions
                                                .Include(t => t.Categories.CategoryDirections)
                                                .Where(t => t.TransactionDate <= today && t.AccountID == MainViewModel.CurrentAccount.AccountID)
                                                .ToList();
                var startingTransactions = allTransaction.Where(t => t.TransactionDate < firstDayOfFirstMonth && t.AccountID == MainViewModel.CurrentAccount.AccountID)
                                                         .ToList();
                int balance = (int)MainViewModel.CurrentAccount.StartBalance;
                foreach (var transaction in startingTransactions)
                {
                    if (transaction.Categories.CategoryDirections.DirectionName == "Kiadás")
                        balance -= (int)transaction.Amount;
                    else
                        balance += (int)transaction.Amount;
                }
                int colbal = balance;

                var transactionsGroupByDate = allTransaction
                                                .Where(t => t.TransactionDate >= firstDayOfFirstMonth)
                                                .GroupBy(t => t.TransactionDate)
                                                .OrderBy(x => x.Key)
                                                .Select(t => new
                                                {
                                                    TransactionDate = t.Key,
                                                    Amount = t.Where(x => x.Categories.CategoryDirections.DirectionName == "Bevétel").Sum(x => x.Amount) -
                                                             t.Where(x => x.Categories.CategoryDirections.DirectionName == "Kiadás").Sum(x => x.Amount)
                                                });

                LineChart.Labels = transactionsGroupByDate.Select(x => x.TransactionDate.ToString("d")).ToArray();
                LineChart.SeriesCollection = new SeriesCollection
                {
                    new StepLineSeries
                    {
                        Title = MainViewModel.CurrentAccount.AccountName,
                        Values = new ChartValues<int>(transactionsGroupByDate.Select(x => colbal += (int)x.Amount).ToArray())
                    }
                };
            }
        }

        public void Refresh()
        {
            throw new NotImplementedException();
        }
    }
}
