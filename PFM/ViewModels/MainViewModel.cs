using System.Linq;
using System.Collections.ObjectModel;
using System.Windows.Input;
using PFM.Models;
using PFM.Commands;
using System;

namespace PFM.ViewModels
{
    /// <summary>
    /// ViewModel which handles all ViewModels of the application
    /// </summary>
    class MainViewModel : BaseViewModel
    {
        private Accounts currentAccount;
        public Accounts CurrentAccount
        {
            get { return currentAccount; }
            set
            {
                if (TransactionViewModel != null)
                {
                    TransactionViewModel.Transactions.Clear();
                    ReportViewModel.RefreshCharts();
                }
                currentAccount = value;
            }
        }

        #region ViewModels

        public ObservableCollection<BaseViewModel> MainViewModels { get; set; }
        public TransactionViewModel TransactionViewModel { get; set; }
        public ReportViewModel ReportViewModel { get; set; }
        public AccountViewModel AccountViewModel { get; set; }
        public CalculationViewModel CalculationViewModel { get; set; }
        public CategoryViewModel CategoryViewModel { get; set; }

        #endregion

        public ObservableCollection<Accounts> Accounts { get; set; }
        
        public ICommand MenuCommand
        {
            get
            {
                return new RelayCommand(
                        p => ChangeMenu((BaseViewModel)p),
                        p => p is BaseViewModel);
            }
        }

        public IBaseViewModel CurrentViewModel { get; set; }
        public PageList ActualPage { get; set; }

        #region Methods
        
        /// <summary>
        /// Initialize all view models for the application
        /// </summary>
        public void InitializeViewModels()
        {
            TransactionViewModel = new TransactionViewModel(this);
            ReportViewModel = new ReportViewModel(this);
            AccountViewModel = new AccountViewModel(this);
            CalculationViewModel = new CalculationViewModel(this);
            CategoryViewModel = new CategoryViewModel(this, CategoryViewModel.InteractionMode.Menu);
            MainViewModels = new ObservableCollection<BaseViewModel>
            {
                TransactionViewModel,
                ReportViewModel,
                AccountViewModel,
                CalculationViewModel,
                CategoryViewModel
            };
            ChangeMenu(MainViewModels[0]);
        }

        /// <summary>
        /// Sets the recently logged in user, and get the default account for that user
        /// </summary>
        /// <param name="userID">The ID of the logged in user</param>
        public void SetUser(int userID)
        {
            using (var dataModel = new DataModel())
            {
                CurrentUser = dataModel.Users.First(u => u.UserID == userID);
                Accounts = new ObservableCollection<Accounts>(dataModel.Accounts.Where(a => a.UserID == CurrentUser.UserID).ToList().OrderBy(a => a.AccountName));
                CurrentAccount = Accounts.First(a => a.Default);
            }
            InitializeViewModels();
        }

        /// <summary>
        /// Registers the change between view models
        /// </summary>
        /// <param name="newVM"></param>
        public void ChangeMenu(BaseViewModel newVM)
        {
            if (!MainViewModels.Contains(newVM))
                MainViewModels.Add(newVM);
            if(CurrentViewModel != null)
                CurrentViewModel.Selected = false;
            CurrentViewModel = MainViewModels.FirstOrDefault(vm => vm == newVM);
            CurrentViewModel.Selected = true;
        }

        #endregion

    }
}
