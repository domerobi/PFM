using System;
using System.Windows.Input;

namespace PFM
{
    class UpdateCommand : ICommand
    {
        private ReportViewModel viewModel;

        public event EventHandler CanExecuteChanged;

        public UpdateCommand(ReportViewModel vm)
        {
            viewModel = vm;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            Inventory itemToUpdate = viewModel.DBInventory.ModifyItem.CreateItem();
            viewModel.DBInventory.UpdateItem(itemToUpdate);
            viewModel.DBInventory.InventoryRecords.Remove(viewModel.DBInventory.SelectedItem);
            viewModel.DBInventory.InventoryRecords.Add(itemToUpdate);
            viewModel.DBInventory.SortInventoryByDate();
            viewModel.DBInventory.ModView.Close();
            viewModel.UpdateCharts();
        }
    }
}
