using PFM.Commands;
using PFM.Models;
using PFM.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using System.Data.Entity;
using System.Collections.ObjectModel;
using LiveCharts.Wpf;
using LiveCharts;

namespace PFM.ViewModels
{
    /// <summary>
    /// The view model representation of Calculation and CalculationData database classes
    /// </summary>
    class CalculationViewModel : BaseViewModel
    {

        #region Private properties

        IWindowService windowService;

        #endregion

        public MainViewModel MainViewModel { get; set; }
        
        #region Filters

        public Calculation CalculationFilter { get; set; }

        public bool SaveCalculation { get; set; }

        public ICommand CalculateCommand { get; set; }
        public ICommand ResetCommand { get; set; }
        #endregion

        #region Display

        public class CalcDataDisplay
        {
            public string CategoryName { get; set; }

            public decimal Average { get; set; }

            public decimal Limit { get; set; }

            public decimal Saved { get; set; }
        }

        public ObservableCollection<CalcDataDisplay> CalculationResult { get; set; }

        public bool HasCalculation { get; set; }

        public ChartesianChartViewModel LineChart { get; set; }

        public int CurrentBalance { get; set; }

        #endregion

        /// <summary>
        /// Constructor of the class
        /// </summary>
        /// <param name="mainViewModel">The MainViewModel which holds the logged in user and the selected account</param>
        public CalculationViewModel(MainViewModel mainViewModel)
        {
            MainViewModel = mainViewModel;
            Initialize();
        }

        /// <summary>
        /// Initialize the viewmodel when it is created
        /// </summary>
        public void Initialize()
        {
            Name = "Kalkuláció készítése";

            windowService = new WindowService();

            CalculateCommand = new RelayCommand(
                    p => Calculate(),
                    p => CanCalculate());

            ResetCommand = new RelayCommand(
                    p => Reset() );

            LineChart = new ChartesianChartViewModel
            {
                Title = "A kalkuláció hatása az egyenlegre",
                Formatter = value => value.ToString("C0")
            };
            
            Reset();
        }

        public bool CanCalculate()
        {
            return CalculationFilter.Amount > 0 &&
                   CalculationFilter.DueDate > DateTime.Today &&
                   (SaveCalculation && !String.IsNullOrEmpty(CalculationFilter.CalculationName) || !SaveCalculation);
        }

        /// <summary>
        /// Calculates from the last six months' average expense, that how can the user save enough money to afford a future
        /// transaction. It lists how much money the user needs to save from each expense category to achieve the goal amount.
        /// </summary>
        public void Calculate()
        {
            decimal cumulativeAmount = 0;
            List<UserCategory> allCategories;
            List<Transactions> transactionsLastSixMonths;
            DateTime startDayOfCalculation;
            DateTime lastDayOfLastMonth;
            CalculationResult.Clear();
            HasCalculation = false;

            // do all the actions that needs database
            using (var db = new DataModel())
            {
                CurrentBalance = (int)db.AccountBalance
                                        .First(ab => ab.AccountID == MainViewModel.CurrentAccount.AccountID)
                                        .Balance
                                        .GetValueOrDefault();
                // if balance included, then set the start amount of calculation to the balance of the current account
                if (CalculationFilter.BalanceIncluded)
                {
                    cumulativeAmount = (decimal)CurrentBalance;
                }

                // set the initial dates
                DateTime lastMonth = DateTime.Today.AddMonths(-1);
                startDayOfCalculation = new DateTime(lastMonth.Year, lastMonth.Month, 1).AddMonths(-5);
                lastDayOfLastMonth = DateTime.Today.AddDays(-(DateTime.Today.Day));

                Console.WriteLine("Before calc: {0} - {1}", startDayOfCalculation, lastDayOfLastMonth);

                // get the transactions for the last six months
                transactionsLastSixMonths = db.Transactions
                                                  .Include(t => t.Categories.CategoryDirections)
                                                  .Where(t => t.TransactionDate >= startDayOfCalculation &&
                                                              t.TransactionDate <= lastDayOfLastMonth &&
                                                              t.AccountID == MainViewModel.CurrentAccount.AccountID)
                                                  .ToList<Transactions>();

                // get all the categories which are not excluded from calculation
                allCategories = db.UserCategory
                                  .Include(uc => uc.Category.CategoryDirections)
                                  .Where(uc => uc.UserID == MainViewModel.CurrentUser.UserID)
                                  .ToList<UserCategory>();

            }

            // get the create date of the first transaction
            DateTime firstTransactionDate = transactionsLastSixMonths.Select(t => t.TransactionDate).Min();

            // if the first transactions date is a later date then startDayOfCalculation 
            // then set the first day to the transaction date
            if (firstTransactionDate > startDayOfCalculation)
                startDayOfCalculation = new DateTime(firstTransactionDate.Year, firstTransactionDate.Month, 1);

            Console.WriteLine("Start: {0}\nEnd: {1}", startDayOfCalculation, lastDayOfLastMonth);

            // get the months between the starting date and the last month
            int monthDifference = CommonFunctionService.MonthDifference(lastDayOfLastMonth, startDayOfCalculation) + 1;

            Console.WriteLine("Month difference: {0}", monthDifference);

            // get the average amount spent by each category
            var averageAmountByCategories = transactionsLastSixMonths
                                    .GroupBy(t => t.Categories.CategoryName)
                                    .Select(t => new
                                    {
                                        Category = t.Key,
                                        CategoryDirection = t.First().Categories.CategoryDirections.DirectionName,
                                        Amount = t.Sum(s => s.Amount) / monthDifference
                                    })
                                    .ToList();

            CalculationFilter.CalculationData.Clear();
            // create calculation data for each category, and initialize the limit with the average amount
            foreach (var category in allCategories)
            {
                var currentCategory = averageAmountByCategories
                                .FirstOrDefault(c => c.Category == category.Category.CategoryName);
                decimal average = 0;
                if (currentCategory != null)
                    average = currentCategory.Amount;

                var newCalcData = new CalculationData
                {
                    CategoryID = category.CategoryID,
                    Average = average,
                    Limit = average
                };
                CalculationFilter.CalculationData.Add(newCalcData);
            }

            // average income in a month
            decimal incomes = averageAmountByCategories
                                .Where(c => c.CategoryDirection == "Bevétel")
                                .Sum(c => c.Amount);

            // average expense in a month
            decimal expenses = averageAmountByCategories
                                .Where(c => c.CategoryDirection == "Kiadás")
                                .Sum(c => c.Amount);

            // average balance in a month
            decimal averageBalance = incomes - expenses;

            // get the difference in months between today and the due date
            int monthToGoal = CommonFunctionService.MonthDifference(CalculationFilter.DueDate, DateTime.Today) - 1;

            Console.WriteLine("{0}*{1}+{2}={3}",monthToGoal, averageBalance, cumulativeAmount,
                                                monthToGoal * averageBalance + cumulativeAmount);
            Console.WriteLine("{0}", CalculationFilter.Amount);

            cumulativeAmount += monthToGoal * averageBalance;
            // if the average savings plus the start balance is reaching the goal amount, then no
            // calculation is needed
            if (cumulativeAmount >= CalculationFilter.Amount)
            {
                windowService.UserMessage("A jelenlegi bevétel-kiadás arány mellett nem szükséges további spórolás a kitűzött cél megvalósításához");
                return;
            }

            // get the priority levels that are not excluded from calculation
            var categoryPriority = allCategories
                                    .Where(uc => uc.Category.CategoryDirections.DirectionName == "Kiadás" &&
                                                 uc.ExcludeFromCalculation == false)
                                    .Select(uc => new
                                    {
                                        Priority = uc.Priority,
                                        CategoryID = uc.CategoryID,
                                        CategoryName = uc.Category.CategoryName,
                                        Percentage = uc.Limit
                                    })
                                    .OrderBy(t => t.Priority)
                                    .ThenByDescending(t => t.Percentage)
                                    .ToList();

            var priorityLevels = categoryPriority
                                    .Select(t => t.Priority)
                                    .Distinct()
                                    .ToList<int>();

            bool goalReached = false;

            // go through each priority level
            foreach (var priority in priorityLevels)
            {
                var currentPriority = categoryPriority
                                        .Where(uc => uc.Priority == priority)
                                        .ToList();
                int cycleCounter = 1;
                double maxPercentage = currentPriority.Max(t => t.Percentage);
                bool exit = false;
                Console.WriteLine("Priority {0}:", priority);
                while (!exit)
                {
                    Console.WriteLine("{0}. round:", cycleCounter);
                    foreach (var category in currentPriority)
                    {
                        // calculate only if we didn't reach the limit
                        if (category.Percentage >= cycleCounter * 0.01)
                        {
                            var currentCalcData = CalculationFilter
                                                    .CalculationData
                                                    .First(cd => cd.CategoryID == category.CategoryID);
                            currentCalcData.Limit -= currentCalcData.Average * 0.01M;

                            // increase the saved amount by the amount we subtracted from limit for each month
                            cumulativeAmount += currentCalcData.Average * 0.01M * monthToGoal;

                            Console.WriteLine("{0}: {1}->{2}", category.CategoryName, currentCalcData.Average, currentCalcData.Limit);

                            // if the saved amount equals or greater than the planned amount, then we are done
                            if(cumulativeAmount >= CalculationFilter.Amount)
                            {
                                exit = true;
                                goalReached = true;
                                break;
                            }
                                
                        }
                    }
                    Console.WriteLine("Amount after {0}. round: {1}", cycleCounter, cumulativeAmount);
                
                    // if we reached the category with the highest percentage then go to next level
                    if (maxPercentage <= cycleCounter * 0.01)
                        exit = true;
                    cycleCounter++;
                }

                // after each level we need to check, if we reached the goal already
                if (goalReached)
                    break;
            }

            if (goalReached)
            {
                // create a collection for display
                foreach (var category in allCategories)
                {
                    // if category is not type of expense then skip
                    if (category.Category.CategoryDirections.DirectionName != "Kiadás")
                        continue;

                    var calcData = CalculationFilter.CalculationData.First(cd => cd.CategoryID == category.CategoryID);
                    var displayCalculationData = new CalcDataDisplay
                    {
                        CategoryName = category.Category.CategoryName,
                        Average = calcData.Average,
                        Limit = calcData.Limit,
                        Saved = calcData.Average - calcData.Limit
                    };

                    CalculationResult.Add(displayCalculationData);
                }
                CreateCalculationReport(incomes, expenses);
            }
            else
            {
                windowService.UserMessage("Nem sikerült eleget spórolni a cél eléréséhez!");
            }

            HasCalculation = CalculationResult.Count > 0;

        }

        public void CreateCalculationReport(decimal averageIncomes, decimal averageExpenses)
        {
            decimal balance;
            decimal calcBalance;

            // first clear the series
            LineChart.Series = new List<Series>();
            LineChart.SeriesCollection = new SeriesCollection();

            // get the actual balance as the startin amount
            using (var db = new DataModel())
            {
                balance = db.AccountBalance
                            .First(ab => ab.AccountID == MainViewModel.CurrentAccount.AccountID)
                            .Balance
                            .GetValueOrDefault();
                
            }

            calcBalance = balance;

            // set the start date to the first day of the month
            var startDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddMonths(1);
            Console.WriteLine("Report:\nStart date: {0}", startDate);

            // get the average expense of the calculated data
            decimal averageCalcExpenses = CalculationResult.Sum(cr => cr.Limit);

            Console.WriteLine("Average income: {0}\nAverage expense: {1}\nCalculated expense: {2}", averageIncomes, averageExpenses, averageCalcExpenses);

            ChartValues<double> normalBalance = new ChartValues<double>();
            ChartValues<double> calculatedBalance = new ChartValues<double>();
            List<string> labels = new List<string>();

            int cycleCounter = 0;
            for (var i = startDate; i <= CalculationFilter.DueDate; i = i.AddMonths(1))
            {
                //LineChart.Labels[cycleCounter] = i.ToString("yyyy MMMM");
                labels.Add(i.ToString("yyyy MMMM"));
                normalBalance.Add((double)balance);
                calculatedBalance.Add((double)calcBalance);
                balance += averageIncomes - averageExpenses;
                calcBalance += averageIncomes - averageCalcExpenses;
                cycleCounter++;
            }

            LineChart.Labels = labels.ToArray();

            LineChart.Series.Add(new LineSeries
            {
                Title = "Egyenleg alakulása",
                Values = normalBalance
            });
            LineChart.Series.Add(new LineSeries
            {
                Title = "Egyenleg alakulása kalkulációval",
                Values = calculatedBalance
            });

            LineChart.SeriesCollection.AddRange(LineChart.Series);

        }

        /// <summary>
        /// Resets the filter fields with following default values:
        ///   - Due             : Today + 6 months
        ///   - Amount          : 0
        ///   - CalculationName : ""
        ///   - SaveCalculation : false
        /// </summary>
        private void Reset()
        {
            CalculationFilter = new Calculation
            {
                AccountID = MainViewModel.CurrentAccount.AccountID,
                DueDate = DateTime.Today.AddMonths(6),
                Amount = 0,
                CalculationName = "",
                CalculationData = new ObservableCollection<CalculationData>()
            };
            SaveCalculation = false;
            CalculationResult = new ObservableCollection<CalcDataDisplay>();
            HasCalculation = false;
        }
    }
}
