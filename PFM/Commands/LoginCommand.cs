using System;

using System.Windows.Forms;
using System.Windows.Input;
using PFM.ViewModels;
using PFM.Views;

namespace PFM
{
    class LoginCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private LoginViewModel mLoginViewModel;
        private MainWindowView mainWindowView;
        
        public LoginCommand(LoginViewModel wvm)
        {
            mLoginViewModel = wvm;
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
                        var newAccount = new AccountViewModel(mLoginViewModel.CurrentUser);
                        if (newAccount.CreateInteractive() == false)
                        {
                            return;
                        }
                        /*
                        CreateAccountView createAccountView = new CreateAccountView
                        {
                            DataContext = new AccountViewModel(null, mLoginViewModel.CurrentUser)
                        };
                        if (createAccountView.ShowDialog() == false)
                            return;
                            */
                    }

                    mainWindowView = new MainWindowView(mLoginViewModel.CurrentUser.UserID);
                    mainWindowView.Show();}
                else
                {
                    MessageBox.Show("Rossz felhasználónév vagy jelszó!");
                    mLoginViewModel.loginPage.clearPassword();
                }
            }
        }
    }
}