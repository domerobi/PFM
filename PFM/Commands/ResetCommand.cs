using System;
using System.Windows.Input;

namespace PFM
{
    class ResetCommand : ICommand
    {
        private InventoryViewModel viewModel;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested += value; }
        }

        public ResetCommand(InventoryViewModel vm)
        {
            viewModel = vm;
        }

        public bool CanExecute(object parameter)
        {
            return viewModel.SearchItem.CanReset();
        }

        public void Execute(object parameter)
        {
            viewModel.SearchItem.Reset();
            viewModel.ReadFromDB();
        }
    }
}
