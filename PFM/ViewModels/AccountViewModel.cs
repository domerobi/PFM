using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace PFM
{
    class AccountViewModel : BaseViewModel
    {
        public Accounts CurrentAccount { get; set; }
        public Users CurrentUser { get; set; }
        public ObservableCollection<Currency_View> Currencies { get; set; }
        
        public Action CloseAction { get; set; }
        public ICommand CreateAccount { get; set; }

        public AccountViewModel(Users user)
        {
            CurrentAccount = new Accounts();
            CurrentUser = user;
            CurrentAccount.UserID = CurrentUser.UserID;
            CurrentAccount.Currency = "HUF";
            CreateAccount = new CreateAccountCommand(this);
            using (var db = new DataModel())
            {
                var currencies = db.Currency_View.ToList();
                Currencies = db.Currency_View.Local;
                Currencies.Add(new Currency_View() { Currency = "--" });
            }
        }

        public bool CanCreate()
        {
            if (String.IsNullOrEmpty(CurrentAccount.AccountName) || String.IsNullOrEmpty(CurrentAccount.Currency) || CurrentAccount.Currency == "--")
                return false;
            return true;
        }

        public bool Create()
        {
            CurrentAccount.CreateDate = DateTime.Now;
            CurrentAccount.LastModify = CurrentAccount.CreateDate;
            using (var db = new DataModel())
            {
                db.Accounts.Add(CurrentAccount);
                int numOfRow = db.SaveChanges();
                if (numOfRow > 0)
                    return true;
                return false;
            }
        }
    }
}
