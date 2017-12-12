using System;
using System.Windows.Input;

namespace PFM
{
    class DeleteCommand : ICommand
    {
        private MainViewModel viewModel;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public DeleteCommand(MainViewModel vm)
        {
            viewModel = vm;
        }

        public bool CanExecute(object parameter)
        {
            return viewModel.DBInventory.CanDeleteItem();
        }

        public void Execute(object parameter)
        {
            // delete item from DB
            viewModel.DBInventory.DeleteFromDB(viewModel.DBInventory.SelectedItem);
            // delete item from list
            viewModel.DBInventory.InventoryRecords.Remove(viewModel.DBInventory.SelectedItem);
            // update charts
            viewModel.UpdateCharts();
        }
    }
}
