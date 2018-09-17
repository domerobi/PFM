using System;
using System.Windows;
using System.Windows.Input;

namespace PFM
{
    class CreateAccountCommand : ICommand
    {
        private AccountViewModel viewModel;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public CreateAccountCommand(AccountViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        
        public bool CanExecute(object parameter)
        {
            return viewModel.CanCreate();
        }

        public void Execute(object parameter)
        {
            if (viewModel.Create())
            {
                MessageBox.Show("A számla sikeresen létrejött!");
                viewModel.CloseAction();
            }
            else
            {
                MessageBox.Show("A számla létrehozása sikertelen!");
            }
        }
    }
}
