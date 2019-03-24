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
        public ObservableCollection<BaseViewModel> MainViewModels { get; set; }
        public TransactionViewModel TransactionViewModel { get; set; }
        public ReportViewModel ReportViewModel { get; set; }
        public AccountViewModel AccountViewModel { get; set; }

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
        
        public void InitializeViewModels()
        {
            TransactionViewModel = new TransactionViewModel(this);
            ReportViewModel = new ReportViewModel(this);
            AccountViewModel = new AccountViewModel(this);
            MainViewModels = new ObservableCollection<BaseViewModel>
            {
                TransactionViewModel,
                ReportViewModel,
                AccountViewModel
            };
            ChangeMenu(MainViewModels[0]);
            //TransactionViewModel.Transactions.CollectionChanged += Transactions_CollectionChanged;
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
