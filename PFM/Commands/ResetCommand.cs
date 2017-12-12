using System;
using System.Windows.Input;

namespace PFM
{
    class ResetCommand : ICommand
    {
        private MainViewModel viewModel;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested += value; }
        }

        public ResetCommand(MainViewModel vm)
        {
            viewModel = vm;
        }

        public bool CanExecute(object parameter)
        {
            return viewModel.DBInventory.SearchItem.CanReset();
        }

        public void Execute(object parameter)
        {
            // reset search to default
            viewModel.DBInventory.SearchItem.Reset();
            // read data from DB
            viewModel.DBInventory.ReadFromDB();
            // update charts
            viewModel.UpdateCharts();
        }
    }
}
