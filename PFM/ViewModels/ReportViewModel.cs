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
using System.ComponentModel;
using PFM.ValueConverters;
using System.Windows.Input;
using PFM.Commands;
using PFM.Services;

namespace PFM.ViewModels
{
    /// <summary>
    /// View model for managing reports in the reports menu
    /// </summary>
    class ReportViewModel : BaseViewModel
    {
        #region ViewModels

        public ObservableCollection<IChartModel> ReportViewModels { get; set; }
        public IChartModel CurrentViewModel { get; set; }
        public MainViewModel MainViewModel { get; set; }

        public CategoryChartViewModel ExpendCategories { get; set; }
        public CategoryChartViewModel IncomeCategories { get; set; }
        public ChartesianChartViewModel StackedSixMonths { get; set; }
        public ChartesianChartViewModel ExpenseIncomeColumn { get; set; }
        public BalanceViewModel BalanceViewModel { get; set; }

        /// <summary>
        /// Chart type for displaying
        /// </summary>
        public enum ChartType
        {
            Income,
            Expense,
            Both
        }
        #endregion

        #region Display

        IWindowService windowService;

        #endregion

        #region Filter

        /// <summary>
        /// Grouping of the current filter
        /// </summary>
        [TypeConverter(typeof(EnumNameConverter))]
        public enum FilterGroup
        {
            [Description("Nincs csoportosítás")]
            None,
            [Description("Naponta")]
            Daily,
            [Description("Havonta")]
            Monthly,
            [Description("Negyedévente")]
            Quarterly,
            [Description("Félévente")]
            HalfYearly,
            [Description("Évente")]
            Yearly
        }

        /// <summary>
        /// Types of the filter
        /// </summary>
        public enum FilterType
        {
            ActualMonth,
            LastMonth,
            Last2Month,
            Last6Month,
            LastYear,
            OwnDates
        }

        /// <summary>
        /// Class to handle filtering a report
        /// </summary>
        public class ReportFilter : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;

            /// <summary>
            /// Type of the filter
            /// </summary>
            public FilterType Type { get; set; }

            /// <summary>
            /// The first transaction's date
            /// </summary>
            public DateTime StartDate { get; set; }

            /// <summary>
            /// The last transaction's date
            /// </summary>
            public DateTime EndDate { get; set; }

            /// <summary>
            /// Collection of filtergroups which are enabled for this filter
            /// </summary>
            public ObservableCollection<FilterGroup> FilterGroups { get; set; }

            /// <summary>
            /// Currently selected filtergroup
            /// </summary>
            public FilterGroup CurrentFilterGroup { get; set; }

            /// <summary>
            /// Only show filtergroups when it has any member
            /// </summary>
            public bool ShowFilterGroups
            {
                get
                {
                    if (FilterGroups.Count > 0)
                        return true;
                    return false;
                }
            }

            /// <summary>
            /// Dates can be changed only when owndates type is selected
            /// </summary>
            public bool CanEditDates
            {
                get
                {
                    if (Type == FilterType.OwnDates)
                        return true;
                    return false;
                }
            }

            /// <summary>
            /// Base ToString overrided to give back the name of the type
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                switch (Type)
                {
                    case FilterType.ActualMonth:
                        return "Aktuális hónap";
                    case FilterType.LastMonth:
                        return "Előző hónap";
                    case FilterType.Last2Month:
                        return "Előző két hónap";
                    case FilterType.Last6Month:
                        return "Előző fél év";
                    case FilterType.LastYear:
                        return "Előző év";
                    case FilterType.OwnDates:
                        return "Egyéni dátumok";
                    default:
                        return "";
                }
            }
        }

        /// <summary>
        /// Class for making the grouping on the X axis of the chart
        /// </summary>
        public class FilterGroupData
        {
            public DateTime StartDate { get; set; }

            public DateTime EndDate { get; set; }

            public string Name { get; set; }
        }

        public ObservableCollection<ReportFilter> ReportFilters { get; set; }

        private ReportFilter currentFilter;
        public ReportFilter CurrentFilter
        {
            get
            {
                return currentFilter;
            }
            set
            {
                currentFilter = value;
                if (currentFilter == null)
                    return;

                if (currentFilter.CurrentFilterGroup == FilterGroup.None && currentFilter.FilterGroups.Count > 0)
                    currentFilter.CurrentFilterGroup = currentFilter.FilterGroups.Min();
            }
        }
        
        public ICommand FilterCommand { get; set; }
        public ICommand ResetCommand { get; set; }

        #endregion

        /// <summary>
        /// Initialize the properties
        /// </summary>
        /// <param name="mainViewModel"></param>
        public ReportViewModel(MainViewModel mainViewModel)
        {
            Name = "Kimutatások";

            MainViewModel = mainViewModel;

            windowService = new WindowService();

            ExpendCategories = new CategoryChartViewModel
            {
                Name = "Kiadások összesítve"
            };
            ExpendCategories.Title = ExpendCategories.Name;

            IncomeCategories = new CategoryChartViewModel
            {
                Name = "Bevételek összesítve"
            };
            IncomeCategories.Title = IncomeCategories.Name;

            StackedSixMonths = new ChartesianChartViewModel
            {
                Name = "Kiadások részletesen",
                Formatter = value => value.ToString("C0"),
                XTitle = "Periódus",
                YTitle = "Összeg"
            };
            StackedSixMonths.Title = StackedSixMonths.Name;

            ExpenseIncomeColumn = new ChartesianChartViewModel
            {
                Name = "Kiadás-Bevétel mérleg",
                Formatter = value => value.ToString("C0"),
                XTitle = "Periódus",
                YTitle = "Összeg"
            };
            ExpenseIncomeColumn.Title = ExpenseIncomeColumn.Name;

            BalanceViewModel = new BalanceViewModel(MainViewModel);

            // Initialize all of the report categories to be displayed
            ReportViewModels = new ObservableCollection<IChartModel>
            {
                ExpendCategories,
                IncomeCategories,
                StackedSixMonths,
                ExpenseIncomeColumn,
                BalanceViewModel
            };
            CurrentViewModel = ReportViewModels[0];

            FilterCommand = new RelayCommand( p => RefreshCharts() );
            ResetCommand = new RelayCommand( p => ResetFilter());

            ResetFilter();
            RefreshCharts();
        }

        /// <summary>
        /// For a specified filter type it gives back the enabled filter groups
        /// </summary>
        /// <param name="filterType">Filter type for which the filter groups are needed</param>
        /// <returns></returns>
        public ObservableCollection<FilterGroup> GetEnabledFilterGroupsByType(FilterType filterType)
        {
            if (CurrentViewModel is CategoryChartViewModel)
                return new ObservableCollection<FilterGroup>();

            if(CurrentViewModel is ChartesianChartViewModel)
            {
                switch (filterType)
                {
                    case FilterType.ActualMonth:
                        return new ObservableCollection<FilterGroup>
                        {
                            FilterGroup.Daily,
                            FilterGroup.Monthly
                        };
                    case FilterType.LastMonth:
                        return new ObservableCollection<FilterGroup>
                        {
                            FilterGroup.Daily,
                            FilterGroup.Monthly
                        };
                    case FilterType.Last2Month:
                        return new ObservableCollection<FilterGroup>
                        {
                            FilterGroup.Monthly
                        };
                    case FilterType.Last6Month:
                        return new ObservableCollection<FilterGroup>
                        {
                            FilterGroup.Monthly,
                            FilterGroup.Quarterly,
                            FilterGroup.HalfYearly
                        };
                    case FilterType.LastYear:
                        return new ObservableCollection<FilterGroup>
                        {
                            FilterGroup.Monthly,
                            FilterGroup.Quarterly,
                            FilterGroup.HalfYearly,
                            FilterGroup.Yearly
                        };
                    case FilterType.OwnDates:
                        return new ObservableCollection<FilterGroup>
                        {
                            FilterGroup.Daily,
                            FilterGroup.Monthly,
                            FilterGroup.Quarterly,
                            FilterGroup.HalfYearly,
                            FilterGroup.Yearly
                        };
                    default:
                        return new ObservableCollection<FilterGroup>();
                }
            }

            if (CurrentViewModel is BalanceViewModel)
            {
                switch (filterType)
                {
                    case FilterType.ActualMonth:
                        return new ObservableCollection<FilterGroup>
                        {
                            FilterGroup.Daily
                        };
                    case FilterType.LastMonth:
                        return new ObservableCollection<FilterGroup>
                        {
                            FilterGroup.Daily
                        };
                    case FilterType.Last2Month:
                        return new ObservableCollection<FilterGroup>
                        {
                            FilterGroup.Daily,
                            FilterGroup.Monthly
                        };
                    case FilterType.Last6Month:
                        return new ObservableCollection<FilterGroup>
                        {
                            FilterGroup.Daily,
                            FilterGroup.Monthly,
                            FilterGroup.Quarterly
                        };
                    case FilterType.LastYear:
                        return new ObservableCollection<FilterGroup>
                        {
                            FilterGroup.Daily,
                            FilterGroup.Monthly,
                            FilterGroup.Quarterly,
                            FilterGroup.HalfYearly
                        };
                    case FilterType.OwnDates:
                        return new ObservableCollection<FilterGroup>
                        {
                            FilterGroup.Daily,
                            FilterGroup.Monthly,
                            FilterGroup.Quarterly,
                            FilterGroup.HalfYearly,
                            FilterGroup.Yearly
                        };
                    default:
                        return new ObservableCollection<FilterGroup>();
                }
            }

            // if something went wrong, give back an empty list
            return new ObservableCollection<FilterGroup>();
        }

        /// <summary>
        /// Initializes the filters for the current view model, and set attributes if parameter is given
        /// </summary>
        /// <param name="reportFilter">If changing view model but want to keep the filter, then it can be set by this parameter</param>
        public void ResetFilter(ReportFilter reportFilter = null)
        {
            ReportFilters = new ObservableCollection<ReportFilter>
            {
                new ReportFilter
                {
                    Type = FilterType.ActualMonth,
                    StartDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1),
                    EndDate = DateTime.Today,
                    FilterGroups = GetEnabledFilterGroupsByType(FilterType.ActualMonth)
                },
                new ReportFilter
                {
                    Type = FilterType.LastMonth,
                    StartDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddMonths(-1),
                    EndDate = DateTime.Today.AddDays(-(DateTime.Today.Day)),
                    FilterGroups = GetEnabledFilterGroupsByType(FilterType.LastMonth)
                },
                new ReportFilter
                {
                    Type = FilterType.Last2Month,
                    StartDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddMonths(-2),
                    EndDate = DateTime.Today.AddDays(-(DateTime.Today.Day)),
                    FilterGroups = GetEnabledFilterGroupsByType(FilterType.Last2Month)
                },
                new ReportFilter
                {
                    Type = FilterType.Last6Month,
                    StartDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddMonths(-6),
                    EndDate = DateTime.Today.AddDays(-(DateTime.Today.Day)),
                    FilterGroups = GetEnabledFilterGroupsByType(FilterType.Last6Month)
                },
                new ReportFilter
                {
                    Type = FilterType.LastYear,
                    StartDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddYears(-1),
                    EndDate = DateTime.Today.AddDays(-(DateTime.Today.Day)),
                    FilterGroups = GetEnabledFilterGroupsByType(FilterType.LastYear)
                },
                new ReportFilter
                {
                    Type = FilterType.OwnDates,
                    StartDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddMonths(-1),
                    EndDate = DateTime.Today.AddDays(-(DateTime.Today.Day)),
                    FilterGroups = GetEnabledFilterGroupsByType(FilterType.OwnDates)
                }
            };

            // if there was no parameter given, there is nothing else to do
            if (reportFilter == null)
            {
                CurrentFilter = ReportFilters.First(rf => rf.Type == FilterType.LastMonth);

                // if there is no element in filter groups then set the current to none
                if (CurrentFilter.FilterGroups.Count == 0)
                    CurrentFilter.CurrentFilterGroup = FilterGroup.None;
                else
                    CurrentFilter.CurrentFilterGroup = CurrentFilter.FilterGroups.Min();

                return;
            }

            // set the current filter to the given parameter
            CurrentFilter = ReportFilters.First(rf => rf.Type == reportFilter.Type);
            if (CurrentFilter.FilterGroups.Contains(reportFilter.CurrentFilterGroup))
                CurrentFilter.CurrentFilterGroup = reportFilter.CurrentFilterGroup;
            else
                CurrentFilter.CurrentFilterGroup = CurrentFilter.FilterGroups.Count == 0 ? FilterGroup.None : CurrentFilter.FilterGroups.Min();
        }

        /// <summary>
        /// Creates a pie chart
        /// </summary>
        /// <param name="chartType">Type of the transactions</param>
        public void SetCategoryPieCharts(ChartType chartType)
        {
            using (var db = new DataModel())
            {
                // let's clear the series first
                var series = new List<Series>();
                string direction = "";
                switch (chartType)
                {
                    case ChartType.Income:
                        direction = "Bevétel";
                        break;
                    case ChartType.Expense:
                        direction = "Kiadás";
                        break;
                    default:
                        Console.WriteLine("Hoppá, hibás típus: {0}", chartType);
                        return;
                }

                // Get the sum of all categories of the last month
                var groupbyCategories = db.Transactions
                                            .Include(t => t.Categories.CategoryDirections)
                                            .Where(t => t.TransactionDate >= CurrentFilter.StartDate &&
                                                        t.TransactionDate <= CurrentFilter.EndDate &&
                                                        t.AccountID == MainViewModel.CurrentAccount.AccountID &&
                                                        t.Categories.CategoryDirections.DirectionName == direction)
                                            .GroupBy(t => t.Categories.CategoryName)
                                            .Select(t => new
                                            {
                                                Category = t.Key,
                                                Amount = t.Sum(c => c.Amount)
                                            }).ToList();


                foreach (var cat in groupbyCategories)
                {
                    series.Add(
                        new PieSeries
                        {
                            Title = cat.Category,
                            Values = new ChartValues<int> { (int)cat.Amount },
                            DataLabels = true,
                            LabelPoint = chartPoint =>
                                                string.Format("{0:C0}", chartPoint.Y)
                        });
                }

                switch (chartType)
                {
                    case ChartType.Income:
                        IncomeCategories.PieSeries = new SeriesCollection();
                        IncomeCategories.PieSeries.AddRange(series);
                        break;
                    case ChartType.Expense:
                        ExpendCategories.PieSeries = new SeriesCollection();
                        ExpendCategories.PieSeries.AddRange(series);
                        break;
                    default:
                        Console.WriteLine("Hoppá, hibás típus: {0}", chartType);
                        break;
                }
            }
        }

        /// <summary>
        /// Creates a column chart
        /// </summary>
        /// <param name="chartType">If expense is given then stacked chart is created, else normal column chart</param>
        public void SetStackedColumnChart(ChartType chartType)
        {
            // let's clear the series first
            var expenseSeries = new List<Series>();
            var incomeSeries = new List<Series>();

            switch (chartType)
            {
                case ChartType.Expense:
                    StackedSixMonths.SeriesCollection = new SeriesCollection();
                    break;
                case ChartType.Both:
                    ExpenseIncomeColumn.SeriesCollection = new SeriesCollection();
                    break;
                default:
                    break;
            }

            List<FilterGroupData> labelData = GetFilterGroupData();

            using (var db = new DataModel())
            {
                var allTransactions = db.Transactions
                                        .Include(t => t.Categories.CategoryDirections)
                                        .Where(t => t.AccountID == MainViewModel.CurrentAccount.AccountID &&
                                                    t.TransactionDate >= CurrentFilter.StartDate &&
                                                    t.TransactionDate <= CurrentFilter.EndDate)
                                        .ToList();

                if (chartType == ChartType.Expense)
                {
                    var categoryList = db.UserCategory
                                         .Include(uc => uc.Category.CategoryDirections)
                                         .Where(uc => uc.UserID == MainViewModel.CurrentUser.UserID &&
                                                      uc.Category.CategoryDirections.DirectionName == "Kiadás")
                                         .ToList();
                    foreach (var category in categoryList)
                    {
                        ChartValues<double> expenseValues = new ChartValues<double>();

                        foreach (var period in labelData)
                        {
                                var currentAmount = allTransactions.Where(t => t.Categories.CategoryName == category.Category.CategoryName &&
                                                                               t.TransactionDate >= period.StartDate &&
                                                                               t.TransactionDate <= period.EndDate).Sum(t => t.Amount);
                        
                                expenseValues.Add((double)currentAmount);
                        }

                        expenseSeries.Add(new StackedColumnSeries
                            {
                                Title = category.Category.CategoryName,
                                Values = expenseValues,
                                DataLabels = false
                            });
                    }
                    StackedSixMonths.SeriesCollection.AddRange(expenseSeries);
                    StackedSixMonths.Labels = new string[labelData.Count];
                }

                if (chartType == ChartType.Both)
                {
                    ChartValues<double> expenseValues = new ChartValues<double>();
                    ChartValues<double> incomeValues = new ChartValues<double>();

                    foreach (var period in labelData)
                    {
                        var expenseAmount = allTransactions.Where(t => t.Categories.CategoryDirections.DirectionName == "Kiadás" &&
                                                                       t.TransactionDate >= period.StartDate &&
                                                                       t.TransactionDate <= period.EndDate).Sum(t => t.Amount);

                        var incomeAmount = allTransactions.Where(t => t.Categories.CategoryDirections.DirectionName == "Bevétel" &&
                                                                      t.TransactionDate >= period.StartDate &&
                                                                      t.TransactionDate <= period.EndDate).Sum(t => t.Amount);

                        expenseValues.Add((double)expenseAmount);
                        incomeValues.Add((double)incomeAmount);
                    }

                    expenseSeries.Add(new ColumnSeries
                    {
                        Title = "Kiadás",
                        Values = expenseValues,
                        DataLabels = false
                    });

                    incomeSeries.Add(new ColumnSeries
                    {
                        Title = "Bevétel",
                        Values = incomeValues,
                        DataLabels = false
                    });
                    
                    ExpenseIncomeColumn.SeriesCollection.AddRange(expenseSeries);
                    ExpenseIncomeColumn.SeriesCollection.AddRange(incomeSeries);
                    ExpenseIncomeColumn.Labels = new string[labelData.Count];
                }
                


                int cycleCounter = 0;
                foreach (var label in labelData)
                {
                    if (chartType == ChartType.Expense)
                        StackedSixMonths.Labels[cycleCounter] = label.Name;
                    if (chartType == ChartType.Both)
                        ExpenseIncomeColumn.Labels[cycleCounter] = label.Name;

                    cycleCounter++;
                }
            }
        }

        /// <summary>
        /// Creates a line chart for balance
        /// </summary>
        public void SetLineChart()
        {
            BalanceViewModel.LineChart.SeriesCollection = new SeriesCollection();
            List<FilterGroupData> labelData = GetFilterGroupData();

            using (var db = new DataModel())
            {
                var allTransaction = db.Transactions
                                       .Include(t => t.Categories.CategoryDirections)
                                       .Where(t => t.AccountID == MainViewModel.CurrentAccount.AccountID &&
                                                   t.TransactionDate >= CurrentFilter.StartDate &&
                                                   t.TransactionDate <= CurrentFilter.EndDate)
                                       .ToList();
                var startingTransactions = db.Transactions
                                             .Include(t => t.Categories.CategoryDirections)
                                             .Where(t => t.AccountID == MainViewModel.CurrentAccount.AccountID &&
                                                         t.TransactionDate < CurrentFilter.StartDate)
                                             .ToList();
                decimal balance = MainViewModel.CurrentAccount.StartBalance;
                foreach (var transaction in startingTransactions)
                {
                    if (transaction.Categories.CategoryDirections.DirectionName == "Kiadás")
                        balance -= transaction.Amount;
                    else
                        balance += transaction.Amount;
                }

                var balanceValues = new ChartValues<double>();

                foreach (var period in labelData)
                {
                    var expenseAmount = allTransaction.Where(t => t.TransactionDate >= period.StartDate &&
                                                                  t.TransactionDate <= period.EndDate &&
                                                                  t.Categories.CategoryDirections.DirectionName == "Kiadás")
                                                      .Sum(t => t.Amount);
                    var incomeAmount = allTransaction.Where(t => t.TransactionDate >= period.StartDate &&
                                                                  t.TransactionDate <= period.EndDate &&
                                                                  t.Categories.CategoryDirections.DirectionName == "Bevétel")
                                                      .Sum(t => t.Amount);
                    balance += incomeAmount - expenseAmount;
                    balanceValues.Add((double)balance);
                }

                var series = new List<Series>();
                series.Add(new StepLineSeries
                {
                    Title = MainViewModel.CurrentAccount.AccountName,
                    Values = balanceValues
                });

                BalanceViewModel.LineChart.SeriesCollection.AddRange(series);
                BalanceViewModel.LineChart.Labels = new string[labelData.Count];

                int cycleCounter = 0;
                foreach (var label in labelData)
                {
                    BalanceViewModel.LineChart.Labels[cycleCounter] = label.Name;
                    cycleCounter++;
                }
                
            }
        }

        /// <summary>
        /// Get the data for the X axis of the chart
        /// </summary>
        /// <returns></returns>
        public List<FilterGroupData> GetFilterGroupData()
        {
            List<FilterGroupData> labelData = new List<FilterGroupData>();

            if (CurrentFilter.CurrentFilterGroup == FilterGroup.Daily)
            {
                for (DateTime i = new DateTime(CurrentFilter.StartDate.Year, CurrentFilter.StartDate.Month, CurrentFilter.StartDate.Day);
                     i <= CurrentFilter.EndDate; i = i.AddDays(1))
                {
                    labelData.Add(new FilterGroupData
                    {
                        StartDate = new DateTime(i.Year, i.Month, i.Day),
                        EndDate = new DateTime(i.Year, i.Month, i.Day),
                        Name = i.ToString("MM.dd")
                    });
                }
            }

            if (CurrentFilter.CurrentFilterGroup == FilterGroup.Monthly)
            {
                // add the first group before the loop, cause it may be not full month
                DateTime nextMonth = CurrentFilter.StartDate.AddMonths(1);
                DateTime endOfMonth = nextMonth.AddDays(-(nextMonth.Day));
                labelData.Add(new FilterGroupData
                {
                    StartDate = CurrentFilter.StartDate,
                    EndDate = endOfMonth,
                    Name = DateTimeFormatInfo.CurrentInfo.GetMonthName(CurrentFilter.StartDate.Month)
                });

                DateTime lastDate = endOfMonth;
                // cycle through only the full months, so leave the last one after the loop
                for (DateTime i = new DateTime(nextMonth.Year, nextMonth.Month, 1);
                     i <= CurrentFilter.EndDate.AddMonths(-1); i = i.AddMonths(1))
                {
                    labelData.Add(new FilterGroupData
                    {
                        StartDate = new DateTime(i.Year, i.Month, i.Day),
                        EndDate = i.AddMonths(1).AddDays(-1),
                        Name = DateTimeFormatInfo.CurrentInfo.GetMonthName(i.Month)
                    });
                    lastDate = i.AddMonths(1).AddDays(-1);
                }

                if (lastDate != currentFilter.EndDate)
                {
                    labelData.Add(new FilterGroupData
                    {
                        StartDate = new DateTime(CurrentFilter.EndDate.Year, CurrentFilter.EndDate.Month, 1),
                        EndDate = CurrentFilter.EndDate,
                        Name = DateTimeFormatInfo.CurrentInfo.GetMonthName(CurrentFilter.EndDate.Month)
                    });
                }
            }

            if (CurrentFilter.CurrentFilterGroup == FilterGroup.Quarterly || CurrentFilter.CurrentFilterGroup == FilterGroup.HalfYearly)
            {
                int increment = 0;
                if (CurrentFilter.CurrentFilterGroup == FilterGroup.Quarterly)
                    increment = 3;
                if (CurrentFilter.CurrentFilterGroup == FilterGroup.HalfYearly)
                    increment = 6;

                DateTime afterFirstPeriod = CurrentFilter.StartDate.AddMonths(increment);
                DateTime endOfFirstPeriod = afterFirstPeriod.AddDays(-(afterFirstPeriod.Day));
                labelData.Add(new FilterGroupData
                {
                    StartDate = CurrentFilter.StartDate,
                    EndDate = endOfFirstPeriod,
                    Name = CurrentFilter.StartDate.ToString("yy.MM") + "-" + endOfFirstPeriod.ToString("yy.MM")
                });

                DateTime lastDate = endOfFirstPeriod;
                // cycle through only the full months, so leave the rest after the loop
                for (DateTime i = new DateTime(afterFirstPeriod.Year, afterFirstPeriod.Month, 1);
                     i <= CurrentFilter.EndDate.AddMonths(-(increment)); i = i.AddMonths(increment))
                {
                    labelData.Add(new FilterGroupData
                    {
                        StartDate = new DateTime(i.Year, i.Month, i.Day),
                        EndDate = i.AddMonths(increment).AddDays(-1),
                        Name = i.ToString("yy.MM") + "-" + i.AddMonths(increment).AddDays(-1).ToString("yy.MM")
                    });
                    lastDate = i.AddMonths(increment).AddDays(-1);
                }

                // process the rest if needed
                if (lastDate != currentFilter.EndDate)
                {
                    labelData.Add(new FilterGroupData
                    {
                        StartDate = lastDate.AddDays(1),
                        EndDate = CurrentFilter.EndDate,
                        Name = lastDate.AddDays(1).ToString("yy.MM") + "-" + CurrentFilter.EndDate.ToString("yy.MM")
                    });
                }

            }

            if (CurrentFilter.CurrentFilterGroup == FilterGroup.Yearly)
            {
                DateTime oneYearLater = CurrentFilter.StartDate.AddYears(1);
                DateTime endOfFirstYear = new DateTime(oneYearLater.Year, 1, 1).AddDays(-1);
                labelData.Add(new FilterGroupData
                {
                    StartDate = CurrentFilter.StartDate,
                    EndDate = endOfFirstYear,
                    Name = CurrentFilter.StartDate.ToString("yyyy")
                });

                DateTime lastDate = endOfFirstYear;
                // cycle through only the full years, so leave the rest after the loop
                for (DateTime i = new DateTime(oneYearLater.Year, oneYearLater.Month, 1);
                     i <= CurrentFilter.EndDate.AddYears(-1); i = i.AddYears(1))
                {
                    labelData.Add(new FilterGroupData
                    {
                        StartDate = new DateTime(i.Year, i.Month, i.Day),
                        EndDate = i.AddYears(1).AddDays(-1),
                        Name = i.ToString("yyyy")
                    });
                    lastDate = i.AddYears(1).AddDays(-1);
                }

                // process the rest if needed
                if (lastDate != currentFilter.EndDate)
                {
                    labelData.Add(new FilterGroupData
                    {
                        StartDate = lastDate.AddDays(1),
                        EndDate = CurrentFilter.EndDate,
                        Name = lastDate.AddDays(1).ToString("yyyy")
                    });
                }
            }

            return labelData;
        }

        /// <summary>
        /// Refresh the chart according to the currently selected tab
        /// </summary>
        public void RefreshCharts()
        {
            if (CurrentViewModel is CategoryChartViewModel)
            {
                if(ExpendCategories == CurrentViewModel as CategoryChartViewModel)
                    SetCategoryPieCharts(ChartType.Expense);
                if (IncomeCategories == CurrentViewModel as CategoryChartViewModel)
                    SetCategoryPieCharts(ChartType.Income);
                return;
            }

            if(CurrentViewModel is ChartesianChartViewModel)
            {
                if (CurrentFilter.Type == FilterType.OwnDates)
                {
                    if (CurrentFilter.CurrentFilterGroup == FilterGroup.Daily)
                    {
                        int dayDifference = CommonFunctionService.DayDifference(CurrentFilter.StartDate, CurrentFilter.EndDate);
                        if(dayDifference > 31)
                        {
                            windowService.UserMessage("Oszlopdiagramnál a csoportosítás típusának a \"Naponta\" csak akkor választható, " +
                                                      "ha a kezdő és végső dátum között maximum 31 nap a különbség!");
                            return;
                        }
                    }

                    if (CurrentFilter.CurrentFilterGroup == FilterGroup.Quarterly || CurrentFilter.CurrentFilterGroup == FilterGroup.HalfYearly)
                    {
                        int monthDifference = CommonFunctionService.MonthDifference(CurrentFilter.EndDate, CurrentFilter.StartDate) + 1;
                        if(monthDifference < 6)
                        {
                            windowService.UserMessage("Oszlopdiagramnál a csoportosítás típusának a \"Negyedévente\" és a \"Félévente\" " +
                                                      "csak akkor választható, ha a kezdő és a végső dátum között legalább " +
                                                      "6 hónap különbség van!");
                            return;
                        }
                    }

                    if (CurrentFilter.CurrentFilterGroup == FilterGroup.Yearly)
                    {
                        int monthDifference = CommonFunctionService.MonthDifference(CurrentFilter.EndDate, CurrentFilter.StartDate) + 1;
                        if (monthDifference < 12)
                        {
                            windowService.UserMessage("Oszlopdiagramnál a csoportosítás típusának az \"Évente\" csak akkor választható, " +
                                                      "ha a kezdő és a végső dátum között legalább 12 hónap különbség van!");
                            return;
                        }
                    }
                }

                if (StackedSixMonths == CurrentViewModel as ChartesianChartViewModel)
                {
                    SetStackedColumnChart(ChartType.Expense);
                }

                if (ExpenseIncomeColumn == CurrentViewModel as ChartesianChartViewModel)
                {
                    SetStackedColumnChart(ChartType.Both);
                }
            }

            if(CurrentViewModel is BalanceViewModel)
            {
                if (CurrentFilter.Type == FilterType.OwnDates)
                {
                    if(CurrentFilter.CurrentFilterGroup == FilterGroup.Monthly)
                    {
                        int monthDifference = CommonFunctionService.MonthDifference(CurrentFilter.EndDate, CurrentFilter.StartDate) + 1;
                        if (monthDifference < 2)
                        {
                            windowService.UserMessage("Vonaldiagramnál a csoportosítás típusának a \"Havonta\" csak akkor választható, " +
                                                      "ha a kezdő és a végső dátum között legalább 2 hónap különbség van!");
                        }
                    }

                    if (CurrentFilter.CurrentFilterGroup == FilterGroup.Quarterly)
                    {
                        int monthDifference = CommonFunctionService.MonthDifference(CurrentFilter.EndDate, CurrentFilter.StartDate) + 1;
                        if (monthDifference < 6)
                        {
                            windowService.UserMessage("Vonaldiagramnál a csoportosítás típusának a \"Negyedévente\" csak akkor választható, " +
                                                      "ha a kezdő és a végső dátum között legalább 6 hónap különbség van!");
                            return;
                        }
                    }

                    if (CurrentFilter.CurrentFilterGroup == FilterGroup.HalfYearly)
                    {
                        int monthDifference = CommonFunctionService.MonthDifference(CurrentFilter.EndDate, CurrentFilter.StartDate) + 1;
                        if (monthDifference < 12)
                        {
                            windowService.UserMessage("Vonaldiagramnál a csoportosítás típusának a \"Félévente\" csak akkor választható, " +
                                                      "ha a kezdő és a végső dátum között legalább 1 év különbség van!");
                            return;
                        }
                    }

                    if (CurrentFilter.CurrentFilterGroup == FilterGroup.Yearly)
                    {
                        int monthDifference = CommonFunctionService.MonthDifference(CurrentFilter.EndDate, CurrentFilter.StartDate) + 1;
                        if (monthDifference < 24)
                        {
                            windowService.UserMessage("Vonaldiagramnál a csoportosítás típusának a \"Évente\" csak akkor választható, " +
                                                      "ha a kezdő és a végső dátum között legalább 2 év különbség van!");
                            return;
                        }
                    }
                }
                SetLineChart();
            }
        }
    }
}
