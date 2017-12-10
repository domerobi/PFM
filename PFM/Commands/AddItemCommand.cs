﻿using System;
using System.Windows.Input;

namespace PFM
{
    class AddItemCommand : ICommand
    {
        // The parent viewmodel, which holds the data we need
        private InventoryViewModel viewModel;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="vm">parent viewmodel</param>
        public AddItemCommand(InventoryViewModel vm)
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
            return viewModel.CanAddItem();
        }

        /// <summary>
        /// Commands to be excecuted when button is clicked
        /// </summary>
        /// <param name="parameter"></param>
        public void Execute(object parameter)
        {
            Inventory itemToAdd = viewModel.ItemType.CreateItem();
            // Insert item to the collection
            viewModel.InventoryRecords.Add(itemToAdd);
            // Sort the collection, so the new element moves to its right place
            viewModel.SortInventoryByDate();
            // Insert item to the database
            viewModel.AddToDB(itemToAdd);
            // Set each input field to default state
            viewModel.ItemType.ClearFields();
        }
    }
}
