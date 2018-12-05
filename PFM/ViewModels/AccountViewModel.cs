using System;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Windows.Input;
using PFM.Models;
using PFM.Services;
using System.Linq;
using PFM.Commands;

namespace PFM.ViewModels
{
    class AccountViewModel : BaseViewModel
    {
        IWindowService windowService;

        public MainViewModel MainViewModel { get; private set; }
        public ObservableCollection<ExchangeRate> Currencies { get; set; }

        public bool NeedDefaultAccount
        {
            get
            {
                if (Accounts.GetDefaultAccount(MainViewModel.CurrentUser) == null)
                    return true;
                return false;
            }
        }
        
        public ICommand CreateCommand { get; set; }
        public ICommand CloseCommand { get; set; }
        public ICommand NewAccountCommand { get; set; }
        public ICommand ModifyCommand { get; set; }

        public AccountViewModel(MainViewModel mainViewModel)
        {
            Initialize();
            MainViewModel = mainViewModel;
        }

        /// <summary>
        /// Constructor for AccountViewModel, it is used to create new account
        /// </summary>
        /// <param name="user">The logged in user</param>
        public AccountViewModel(Users user)
        {
            Initialize();
            using (var db = new DataModel())
            {
                MainViewModel = new MainViewModel();
                MainViewModel.CurrentUser = user;
                MainViewModel.CurrentAccount = new Accounts
                {
                    UserID = user.UserID,
                    StartBalance = 0,
                    Currency = "HUF",
                    Default = Accounts.GetDefaultAccount(MainViewModel.CurrentUser) == null
                };
                MainViewModel.Accounts = new ObservableCollection<Accounts>();
            }
        }

        public void Initialize()
        {
            Name = "Számlák kezelése";
            windowService = new WindowService();

            // Initialize commands
            CreateCommand = new RelayCommand(
                    p => Create(p as IClosable),
                    p => CanCreate());

            CloseCommand = new RelayCommand(
                    p => windowService.CloseWindow(p as IClosable));

            NewAccountCommand = new RelayCommand(
                    p => CreateInteractive());

            ModifyCommand = new RelayCommand(
                    p => Modify(),
                    p => CanCreate());

            using (var db = new DataModel())
            {
                // load currencies
                db.ExchangeRate.Load();
                Currencies = db.ExchangeRate.Local;
            }
        }
        

        public bool CanCreate()
        {
            if (MainViewModel.CurrentAccount == null || String.IsNullOrEmpty(MainViewModel.CurrentAccount.AccountName) || 
                String.IsNullOrEmpty(MainViewModel.CurrentAccount.Currency))
                return false;
            return true;
        }

        public bool Create()
        {
            MainViewModel.CurrentAccount.CreateDate = DateTime.Now;
            MainViewModel.CurrentAccount.LastModify = MainViewModel.CurrentAccount.CreateDate;
            using (var db = new DataModel())
            {
                var defaultAccount = Accounts.GetDefaultAccount(MainViewModel.CurrentUser);
                if (MainViewModel.CurrentAccount.Default && defaultAccount != null)
                {
                    db.Accounts.First(a => a.AccountID == defaultAccount.AccountID).Default = false;
                }
                db.Accounts.Add(MainViewModel.CurrentAccount);
                int numOfRow = db.SaveChanges();
                return numOfRow > 0;
            }
        }

        public void Create(IClosable window)
        {
            if(!MainViewModel.CurrentAccount.Default && Accounts.GetDefaultAccount(MainViewModel.CurrentUser) == null)
            {
                windowService.UserMessage("Az első számlát kötelező alapértelmezettként megjelölni!");
                return;
            }
            if (Create())
                windowService.UserMessage("A számla sikeresen létrejött!");
            else
                windowService.UserMessage("A számla létrehozása sikertelen!");
            window.Close();
        }

        public void Modify()
        {
            if (MainViewModel.CurrentAccount.IsModified())
            {
                using (var db = new DataModel())
                {
                    var defaultAccount = Accounts.GetDefaultAccount(MainViewModel.CurrentUser);
                    if (MainViewModel.CurrentAccount.Default && defaultAccount.AccountID != MainViewModel.CurrentAccount.AccountID)
                    {
                        defaultAccount.Default = false;
                        db.Entry(defaultAccount).State = EntityState.Modified;
                    }
                    var refAccount = db.Accounts.First(a => a.AccountID == MainViewModel.CurrentAccount.AccountID);
                    db.Entry(refAccount).CurrentValues.SetValues(MainViewModel.CurrentAccount);
                    db.SaveChanges();
                    MainViewModel.Accounts = new ObservableCollection<Accounts>(
                                                db.Accounts.Where(a => a.UserID == MainViewModel.CurrentUser.UserID)
                                                           .ToList().OrderBy(a => a.AccountName));
                    MainViewModel.CurrentAccount = MainViewModel.Accounts
                                                                .First(a => a.AccountID == refAccount.AccountID);
                }
            }
        }
        
        

        public bool CreateInteractive()
        {
            var newAccount = new AccountViewModel(MainViewModel.CurrentUser);
            windowService.OpenCreateAccountWindow(newAccount);
            if (newAccount.MainViewModel.CurrentAccount.AccountID > 0)
            {
                MainViewModel.Accounts.Add(newAccount.MainViewModel.CurrentAccount);
                return true;
            }
            return false;
        }
    }
}
