using System.Data.SqlClient;
using System.Windows.Input;
using System.Linq;
using System;

namespace PFM
{
    class LoginViewModel : StartUpViewModel
    {
        public ICommand LoginCommand { get; set; }
        public ICommand RegisterCommand { get; set; }

        public Users CurrentUser { get; set; }
        //public string UserName { get; set; }

        public LoginPage loginPage { get; set; }
        
        public IDataBase DataBase { get; set; }

        public LoginViewModel(LoginPage loginPage)
        {
            //DataBase = new CloudDatabase();
            CurrentUser = new Users();
            CurrentUser.Username = loginPage.UserNameTB.Text;
            mPage = loginPage;
            this.loginPage = loginPage;
            ActualPage = Pages.Login;
            LoginCommand = new LoginCommand(this);
            RegisterCommand = new NavigateCommand(this);
            
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
        
    }
}
