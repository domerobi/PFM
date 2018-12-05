using System.Data.SqlClient;
using System.Windows.Input;
using System.Linq;
using System;
using System.Collections.ObjectModel;
using System.Data.Entity;
using PFM.Models;
using PFM.Pages;

namespace PFM.ViewModels
{
    class LoginViewModel : StartUpViewModel
    {
        public ICommand LoginCommand { get; set; }
        public ICommand RegisterCommand { get; set; }

        public ObservableCollection<Users> Users { get; set; }

        public Users CurrentUser { get; set; }
        //public string UserName { get; set; }

        public LoginPage loginPage { get; set; }
        
        public IDataBase DataBase { get; set; }

        public LoginViewModel()
        {
            ActualPage = PageList.Login;
            LoginCommand = new LoginCommand(this);
            RegisterCommand = new NavigateCommand(this);
            using (var db = new DataModel())
            {
                db.Users.Load();
                var cat = db.CategoryDirections.ToList();
                foreach (var catdir in cat)
                {
                    Console.WriteLine(catdir.DirectionName + ":");
                    foreach (var category in catdir.Categories)
                    {
                        Console.WriteLine("\t" + category.CategoryName);
                    }
                }
            }
        }

        public string GetPassword()
        {
            return loginPage.getPassword();
        }

        public bool CanLogin()
        {
            if (CurrentUser.Username == "")
                return false;
            if (GetPassword() == "")
                return false;
            return true;
        }

        public bool Login()
        {
            using (var db = new DataModel())
            {
                string password = SHA.GenerateSHA256String(GetPassword());
                string username = CurrentUser.Username;
                Users tempUser = db.Users
                                .Include("Accounts")
                                .Where(us => us.Username == username && us.Password == password)
                                .FirstOrDefault();
                if (tempUser != null)
                {
                    CurrentUser = tempUser;
                    return true;
                }
                return false;
            }
        }

        public void SetLoginPage(LoginPage loginPage)
        {
            this.loginPage = loginPage;
            mPage = loginPage;
            CurrentUser = new Users
            {
                Username = loginPage.UserNameTB.Text
            };
        }
        
    }
}
