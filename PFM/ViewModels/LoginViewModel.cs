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
    /// <summary>
    /// View model for handling the login procedure
    /// </summary>
    class LoginViewModel : StartUpViewModel
    {
        public ICommand LoginCommand { get; set; }
        public ICommand RegisterCommand { get; set; }

        public ObservableCollection<Users> Users { get; set; }

        public Users CurrentUser { get; set; }
        

        public LoginPage loginPage { get; set; }

        /// <summary>
        /// Initialize properties
        /// </summary>
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

        /// <summary>
        /// Gets the typed in password
        /// </summary>
        /// <returns></returns>
        public string GetPassword()
        {
            return loginPage.getPassword();
        }

        /// <summary>
        /// Decides if each property is filled correctly to login
        /// </summary>
        /// <returns></returns>
        public bool CanLogin()
        {
            if (CurrentUser.Username == "")
                return false;
            if (GetPassword() == "")
                return false;
            return true;
        }

        /// <summary>
        /// Logging in with the given credentials
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Sets the login page
        /// </summary>
        /// <param name="loginPage"></param>
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
