using System;
using System.Windows;
using System.Windows.Input;

namespace PFM
{
    class DeleteCommand : ICommand
    {
        private ReportViewModel viewModel;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public DeleteCommand(ReportViewModel vm)
        {
            viewModel = vm;
        }

        public bool CanExecute(object parameter)
        {
            return viewModel.DBInventory.CanDeleteItem();
        }

        public void Execute(object parameter)
        {
            MessageBoxResult result = MessageBox.Show("Biztosan törli a tételt?", "Tétel törlése", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if(result == MessageBoxResult.Yes)
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
}
