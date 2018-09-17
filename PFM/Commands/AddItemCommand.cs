using System;
using System.Windows.Input;

namespace PFM
{
    class AddItemCommand : ICommand
    {
        // The parent viewmodel, which holds the data we need
        private ReportViewModel viewModel;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="vm">parent viewmodel</param>
        public AddItemCommand(ReportViewModel vm)
        {
            viewModel = vm;
        }

        /// <summary>
        /// Check whether required fields are filled or not
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool CanExecute(object parameter)
        {
            return viewModel.DBInventory.CanAddItem();
        }

        /// <summary>
        /// Commands to be excecuted when button is clicked
        /// </summary>
        /// <param name="parameter"></param>
        public void Execute(object parameter)
        {
            // generate new item id
            viewModel.DBInventory.GetNextItemID();
            Inventory itemToAdd = viewModel.DBInventory.ItemType.CreateItem();
            // Insert item to the collection
            viewModel.DBInventory.InventoryRecords.Add(itemToAdd);
            // Sort the collection, so the new element moves to its right place
            viewModel.DBInventory.SortInventoryByDate();
            // Insert item to the database
            viewModel.DBInventory.AddToDB(itemToAdd);
            // Set each input field to default state
            viewModel.DBInventory.ItemType.ClearFields();
            // Update charts
            viewModel.UpdateCharts();
        }
    }
}
