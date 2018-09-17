using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;

namespace PFM
{
    class RegisterCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private SignUpViewModel mSignUpViewModel;

        public RegisterCommand(SignUpViewModel wvm)
        {
            mSignUpViewModel = wvm;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if (mSignUpViewModel.CanRegister())
            {
                if (!mSignUpViewModel.UserAlreadyExists())
                {
                    if(mSignUpViewModel.GetPassword() == mSignUpViewModel.GetChkPassword())
                    {
                        Users user = new Users();
                        user.Username = mSignUpViewModel.UserName;
                        user.FirstName = mSignUpViewModel.FirstName;
                        user.LastName = mSignUpViewModel.LastName;
                        user.Email = mSignUpViewModel.Email;
                        user.Password = SHA.GenerateSHA256String(mSignUpViewModel.GetPassword());
                        if(mSignUpViewModel.Register(user) > 0)
                        {
                            MessageBox.Show("Sikeres regisztráció!");
                            mSignUpViewModel.mPage.NavigationService.Navigate(new LoginPage(user.Username));
                        }
                        else
                        {
                            MessageBox.Show("A regisztráció sikertelen!");
                        }
                    }
                    else
                    {
                        MessageBox.Show("A két jelszó nem egyezik meg!");
                        mSignUpViewModel.ClearPasswords();
                    }
                }
                else
                {
                    MessageBox.Show("Ilyen felhasználónév már létezik!");
                    mSignUpViewModel.UserName = "";
                }
            }
            else
            {
                MessageBox.Show("Van még kitöltetlen mező!");
            }
        }
    }
}