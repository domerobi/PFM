using System;
using System.Windows.Input;

namespace PFM
{
    class CancelCommand : ICommand
    {
        private InventoryViewModel viewModel;

        public event EventHandler CanExecuteChanged;

        public CancelCommand(InventoryViewModel vm)
        {
            viewModel = vm;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            viewModel.ModView.Close();
        }
    }
}
