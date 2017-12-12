using System;
using System.Windows.Input;
using PFM.Views;

namespace PFM
{
    class ModifyCommand : ICommand
    {
        private InventoryViewModel viewModel;
        
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public ModifyCommand(InventoryViewModel vm)
        {
            viewModel = vm;
        }

        public bool CanExecute(object parameter)
        {
            return viewModel.CanModifyItem();
        }

        public void Execute(object parameter)
        {
            viewModel.OpenModifyItemDialog();
        }
    }
}
