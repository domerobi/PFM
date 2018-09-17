using System;
using System.Windows;
using System.Windows.Input;

namespace PFM
{
    class LoginCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private LoginViewModel mLoginViewModel;
        private MainWindowView mainWindowView;
        private LoginPage loginPage;

        public LoginCommand(LoginViewModel wvm)
        {
            mLoginViewModel = wvm;
            loginPage = (LoginPage)mLoginViewModel.mPage;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if (mLoginViewModel.CanLogin())
            {
                if(mLoginViewModel.Login())
                {
                    //loginPage.clearPassword();
                    //loginPage.clearUserName();

                    if (mLoginViewModel.CurrentUser.Accounts.Count == 0)
                    {
                        MessageBox.Show("Jelenleg nincs számlája, kérem hozzon létre egyet!", "Hiányzó számla");
                        CreateAccountView createAccountView = new CreateAccountView(mLoginViewModel.CurrentUser);
                        createAccountView.Show();

                    }

                    mainWindowView = new MainWindowView(mLoginViewModel.CurrentUser.UserID);
                    mainWindowView.DataContext = new MainViewModel(mLoginViewModel.CurrentUser.UserID);
                    mainWindowView.Show();
                        
                    
                    
                }
                else
                {
                    MessageBox.Show("Rossz felhasználónév vagy jelszó!");
                    loginPage.clearPassword();
                }
            }
        }
    }
}