using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;

namespace PFM
{
    class NavigateCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private StartUpViewModel mViewModel;

        public NavigateCommand(StartUpViewModel wvm)
        {
            mViewModel = wvm;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if((string)parameter == "back")
            {
                mViewModel.mPage.NavigationService.Navigate(new LoginPage());
            }
            else
            {
                mViewModel.mPage.NavigationService.Navigate(new SignUpPage());
            }
            
        }
    }
}