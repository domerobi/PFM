using System;
using System.Windows.Input;

namespace PFM
{
    class SearchCommand : ICommand
    {
        // The parent viewmodel, which holds the data we need
        private ReportViewModel viewModel;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        #region Constructor

        public SearchCommand(ReportViewModel vm)
        {
            viewModel = vm;
        }

        #endregion

        public bool CanExecute(object parameter)
        {
            return viewModel.DBInventory.SearchItem.CanSearch();
        }

        public void Execute(object parameter)
        {
            // read from DB with filters
            viewModel.DBInventory.ReadFromDB();
            // update charts
            viewModel.UpdateCharts();
        }
    }
}
