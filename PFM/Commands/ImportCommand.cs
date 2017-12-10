using System;
using System.Windows.Input;

namespace PFM
{
    class ImportCommand : ICommand
    {
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        // The parent viewmodel, which holds the data we need
        private InventoryViewModel viewModel;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="vm">parent viewmodel</param>
        public ImportCommand(InventoryViewModel vm)
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
            return viewModel.CanImportItem();
        }

        /// <summary>
        /// Commands to be excecuted when button is clicked
        /// </summary>
        /// <param name="parameter"></param>
        public void Execute(object parameter)
        {
            viewModel.ImportFromExcel();
            // Insert item to the collection
            //viewModel.InventoryRecords.Add(viewModel.ItemType.CreateItem());

            // Insert item to the database
            
            

            // Sort the collection, so the new element moves to its right place
            viewModel.SortInventoryByDate();
            // Set each input field to default state
            //viewModel.ItemType.ClearFields();
        }
    }
}
