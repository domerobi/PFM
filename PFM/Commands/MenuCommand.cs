using System;
using System.Windows.Input;

namespace PFM
{
    class MenuCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private MainViewModel mMainViewModel;

        public MenuCommand(MainViewModel mainViewModel)
        {
            mMainViewModel = mainViewModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            switch ((string)parameter)
            {
                case "NewAccount":
                    mMainViewModel.ActualPage = Pages.NewAccount;
                    break;
                case "Reports":
                    mMainViewModel.ActualPage = Pages.Reports;
                    break;
                case "Properties":
                    mMainViewModel.ActualPage = Pages.Properties;
                    break;
                default:
                    mMainViewModel.ActualPage = Pages.Reports;
                    break;
            }
        }
    }
}
