using System;
using System.Windows.Input;

namespace PFM
{
    class SearchCommand : ICommand
    {
        // The parent viewmodel, which holds the data we need
        private InventoryViewModel viewModel;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        #region Constructor

        public SearchCommand(InventoryViewModel vm)
        {
            viewModel = vm;
        }

        #endregion

        public bool CanExecute(object parameter)
        {
            return viewModel.SearchItem.CanSearch();
        }

        public void Execute(object parameter)
        {
            viewModel.ReadFromDB();
        }
    }
}
