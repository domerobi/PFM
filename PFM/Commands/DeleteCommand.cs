using System;
using System.Windows.Input;

namespace PFM
{
    class DeleteCommand : ICommand
    {
        private InventoryViewModel viewModel;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public DeleteCommand(InventoryViewModel vm)
        {
            viewModel = vm;
        }

        public bool CanExecute(object parameter)
        {
            return viewModel.CanDeleteItem();
        }

        public void Execute(object parameter)
        {
            viewModel.DeleteFromDB(viewModel.SelectedItem);
            viewModel.InventoryRecords.Remove(viewModel.SelectedItem);
        }
    }
}
