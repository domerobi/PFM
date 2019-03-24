using System;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using PFM.Models;
using System.Windows.Input;
using PFM.Commands;
using PFM.Services;

namespace PFM.ViewModels
{
    class TransactionViewModel : BaseTransactionViewModel
    {
        #region private Attributes

        CategoryDirections searchCategoryDirections;
        IWindowService windowService;

        #endregion

        #region public Properties

        public ObservableCollection<Transactions> Transactions { get; set; }
        public ObservableCollection<Transactions> AllTransaction { get; set; }
        public Transactions CurrentTransaction { get; set; }
        public Transactions SelectedTransaction { get; set; }

        public MainViewModel MainViewModel { get; set; }


        public ICommand AddItemCommand { get; set; }
        public ICommand ModifyCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand ImportCommand { get; set; }

        #endregion

        #region public Properties for filter

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public CategoryDirections SearchCategoryDirection
        {
            get { return searchCategoryDirections; }
            set
            {
                searchCategoryDirections = value;
                SearchCategory = SearchCategoryDirection.Categories.Where(c => c.CategoryID < 1).First();
            }
        }
        public Categories SearchCategory { get; set; }
        public ICommand SearchCommand { get; set; }
        public ICommand ResetCommand { get; set; }

        #endregion

        /// <summary>
        /// Constructor for the Transaction ViewModel
        /// </summary>
        public TransactionViewModel(MainViewModel mainViewModel)
        {
            MainViewModel = mainViewModel;
            Initialize();
        }

        public void Initialize()
        {
            Name = "Tranzakciók";
            windowService = new WindowService();

            // Creating Commands
            AddItemCommand = new RelayCommand(
                    p => Create(),
                    p => CanCreate());
            ModifyCommand = new RelayCommand(
                    p => Modify(),
                    p => CanModify());
            DeleteCommand = new RelayCommand(
                    p => Delete(),
                    p => CanModify());
            SearchCommand = new RelayCommand(
                    p => Search(null),
                    p => CanSearch());
            ResetCommand = new RelayCommand(
                    p => ResetSearchFilter());
            ImportCommand = new RelayCommand(
                    p => ImportFromExcel());

            // Initialize available categories
            InitializeCategories();

            // Initialize the new transaction object
            InitializeNewTransaction();

            // Set the filters to basic state and get the related transactions
            ResetSearchFilter();
            Search(null);
        }

        private void Delete()
        {
            if (windowService.ConfirmDelete())
            {
                using (var db = new DataModel())
                {
                    var tmp = db.Transactions.Find(SelectedTransaction.TransactionID);
                    db.Transactions.Remove(tmp);
                    db.SaveChanges();
                    Transactions.Remove(SelectedTransaction);
                    SelectedTransaction = null;
                }
                Search(null);
            }
        }

        public bool CanCreate()
        {
            if (SelectedCategoryDirection.DirectionID < 1 || SelectedCategory.CategoryID < 1 || CurrentTransaction.Amount <= 0
                || CurrentTransaction.TransactionDate == null)
            {
                return false;
            }
            return true;
        }

        public void Create()
        {
            using (var db = new DataModel())
            {
                var category = db.Categories.Include(c => c.CategoryDirections).First(c => c.CategoryID == SelectedCategory.CategoryID);
                CurrentTransaction.CategoryID = category.CategoryID;
                CurrentTransaction.AccountID = MainViewModel.CurrentAccount.AccountID;
                CurrentTransaction.CreateDate = DateTime.Now;
                //db.Entry(CurrentAccount).State = EntityState.Unchanged;
                //db.Entry(category).State = EntityState.Unchanged;
                db.Transactions.Add(CurrentTransaction);
                Transactions.Add(CurrentTransaction);
                db.SaveChanges();
            }
            InitializeNewTransaction();
            Search(null);
        }

        public bool CanModify()
        {
            if (SelectedTransaction == null)
                return false;
            return true;
        }

        public void Modify()
        {
            windowService.OpenModifyTransactionWindow(new ModifyTransactionViewModel(SelectedTransaction));
            Search(SelectedTransaction);
        }

        public void InitializeNewTransaction()
        {
            DateTime transactionDate = DateTime.Today;
            if (CurrentTransaction != null)
            {
                transactionDate = CurrentTransaction.TransactionDate;
            }
            CurrentTransaction = new Transactions
            {
                Amount = 0,
                TransactionDate = transactionDate
            };
            SelectedCategoryDirection = CategoryDirections.First(cd => cd.DirectionID < 1);
        }

        private void ResetSearchFilter()
        {
            StartDate = new DateTime(DateTime.Today.Year, DateTime.Today.AddMonths(-1).Month, 1);
            EndDate = DateTime.Today;
            SearchCategoryDirection = CategoryDirections.First(cd => cd.DirectionID == 0);
        }

        private bool CanSearch()
        {
            if (StartDate == null && EndDate == null && SearchCategoryDirection.DirectionID == 0 && SearchCategory.CategoryID < 1)
                return false;
            return true;
        }

        private void Search(Transactions selected)
        {
            // calculate the balance after each transaction without filtering the categories
            CalculateCumulativeBalance(StartDate, EndDate);
            using (var db = new DataModel())
            {
                Transactions = new ObservableCollection<Transactions>(
                                     AllTransaction
                                       .Where(t => (SearchCategoryDirection.DirectionID == 0 ||
                                                    t.Categories.CategoryDirections.DirectionID == SearchCategoryDirection.DirectionID) &&
                                                   (SearchCategory.CategoryID < 1 || t.CategoryID == SearchCategory.CategoryID)));
            }

            if (selected != null)
            {
                SelectedTransaction = Transactions.FirstOrDefault(t => t.TransactionID == selected.TransactionID);
            }
        }

        /// <summary>
        /// Calculates the balance after each transaction
        /// </summary>
        /// <param name="startDate">The first date to calculate from</param>
        private void CalculateCumulativeBalance(DateTime startDate, DateTime endDate)
        {
            // calculate the balance on StartDate
            decimal cumBalance = GetBalanceOnDate(startDate);

            using (var db = new DataModel())
            {
                db.Transactions.Include(t => t.Categories)
                    .Include(c => c.Categories.CategoryDirections)
                    .Where(td => td.TransactionDate >= startDate &&
                                 td.TransactionDate <= endDate &&
                                 td.AccountID == MainViewModel.CurrentAccount.AccountID)
                    .Load();
                AllTransaction = new ObservableCollection<Transactions>(db.Transactions.Local.OrderBy(t => t.TransactionDate)
                                                                                             .ThenBy(t => t.CreateDate)
                                                                                             .ThenBy(t => t.TransactionID));
            }

            foreach (var transaction in AllTransaction)
            {
                if (transaction.Categories.CategoryDirections.DirectionName == "Kiadás")
                    cumBalance -= transaction.Amount;
                else
                    cumBalance += transaction.Amount;

                // store the balance after the current transaction
                transaction.CurrentBalance = cumBalance;
            }

        }

        /// <summary>
        /// Returns the starting balance on a specified date
        /// </summary>
        /// <param name="refDate">The date for which the balance is calculated</param>
        /// <returns>The starting balance on refDate</returns>
        private decimal GetBalanceOnDate(DateTime refDate)
        {
            using (var db = new DataModel())
            {
                var transactionsBeforeRef = db.Transactions
                                              .Include(t => t.Categories.CategoryDirections)
                                              .Where(t => t.TransactionDate < refDate &&
                                                          t.AccountID == MainViewModel.CurrentAccount.AccountID)
                                              .ToList();

                // we start the calculation from the beginning -> startbalance
                decimal balance = MainViewModel.CurrentAccount.StartBalance;

                // add or subtract each transaction's amount
                foreach (var transaction in transactionsBeforeRef)
                {
                    if (transaction.Categories.CategoryDirections.DirectionName == "Kiadás")
                        balance -= transaction.Amount;
                    else
                        balance += transaction.Amount;
                }

                return balance;
            }
        }

        public void ImportFromExcel()
        {
            var fileName = windowService.GetImportFileName();
            if (fileName != "")
            {
                Microsoft.Office.Interop.Excel.Application excelApp = new Microsoft.Office.Interop.Excel.Application();
                //Static File From Base Path...........
                //Microsoft.Office.Interop.Excel.Workbook excelBook = excelApp.Workbooks.Open(AppDomain.CurrentDomain.BaseDirectory + "TestExcel.xlsx", 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
                //Dynamic File Using Uploader...........
                Microsoft.Office.Interop.Excel.Workbook excelBook = excelApp.Workbooks.Open(fileName.ToString(), 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
                Microsoft.Office.Interop.Excel.Worksheet excelSheet = (Microsoft.Office.Interop.Excel.Worksheet)excelBook.Worksheets.get_Item(1); ;
                Microsoft.Office.Interop.Excel.Range excelRange = excelSheet.UsedRange;

                using (var db = new DataModel())
                {
                    int rowCnt = 0;

                    for (rowCnt = 2; rowCnt <= excelRange.Rows.Count; rowCnt++)
                    {
                        Transactions newTransaction = new Transactions();

                        // imported transactions are connected to the current account
                        newTransaction.AccountID = MainViewModel.CurrentAccount.AccountID;

                        // set the values directly to the new transaction object
                        newTransaction.TransactionDate = DateTime.FromOADate((double)((excelRange.Cells[rowCnt, 1] as Microsoft.Office.Interop.Excel.Range).Value2));
                        newTransaction.Amount = Convert.ToDecimal((excelRange.Cells[rowCnt, 4] as Microsoft.Office.Interop.Excel.Range).Value2);
                        newTransaction.Comment = (string)(excelRange.Cells[rowCnt, 5] as Microsoft.Office.Interop.Excel.Range).Value2;

                        string categoryDirection = (string)(excelRange.Cells[rowCnt, 2] as Microsoft.Office.Interop.Excel.Range).Value2;
                        string categoryName = (string)(excelRange.Cells[rowCnt, 3] as Microsoft.Office.Interop.Excel.Range).Value2;

                        // search for the category in the database
                        var newCategory = db.Categories.Include(c => c.CategoryDirections).FirstOrDefault(c => c.CategoryName == categoryName);

                        // if we don't find it in the database, then create it
                        if (newCategory == null)
                        {
                            var newCategoryDirection = db.CategoryDirections.FirstOrDefault(cd => cd.DirectionName == categoryDirection);

                            // we assume, that only existing categorydirections are imported -> no check for categorydirection
                            newCategory = new Categories();
                            newCategory.CategoryDirectionID = newCategoryDirection.DirectionID;
                            newCategory.CategoryName = categoryName;
                            db.Categories.Add(newCategory);
                        }

                        newTransaction.Categories = newCategory;
                        newTransaction.CreateDate = DateTime.Now;
                        db.Transactions.Add(newTransaction);
                        Transactions.Add(newTransaction);
                    }
                    Transactions = new ObservableCollection<Transactions>(Transactions.OrderBy(t => t.TransactionDate));
                    db.SaveChanges();

                }

                excelBook.Close(true, null, null);
                excelApp.Quit();
            }
        }
    }
}
