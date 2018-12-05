using System.Windows.Input;
using PFM.Models;
using PFM.Commands;
using System;
using System.Linq;
using PFM.Services;

namespace PFM.ViewModels
{
    class ModifyTransactionViewModel : BaseTransactionViewModel
    {
        #region public Attributes

        public Transactions SelectedTransaction { get; set; }
        public Transactions CurrentTransaction { get; set; }
        
        #endregion

        #region Commands

        public ICommand ModifyCommand { get; set; }
        public ICommand CloseCommand { get; set; }

        #endregion

        IWindowService windowService;

        public ModifyTransactionViewModel(Transactions transaction)
        {
            InitializeCategories();

            windowService = new WindowService();

            SelectedTransaction = transaction;

            // Setting the given values to bounded properties
            CurrentTransaction = new Transactions();
            CurrentTransaction.Copy(transaction);
            
            SelectedCategoryDirection = CategoryDirections.First(cd => cd.DirectionID == SelectedTransaction.Categories.CategoryDirections.DirectionID);
            SelectedCategory = SelectedCategoryDirection.Categories.First(c => c.CategoryID == SelectedTransaction.CategoryID);

            // Creating Commands
            ModifyCommand = new RelayCommand(
                    p => ModifyTransaction(p as IClosable),
                    p => CanModify());
            CloseCommand = new RelayCommand(
                    p => windowService.CloseWindow(p as IClosable));
        }

        private bool CanModify()
        {
            if (CurrentTransaction.Amount <= 0 || CurrentTransaction.TransactionDate == null || SelectedCategory.CategoryID < 1 
                || SelectedCategoryDirection.DirectionID < 1)
            {
                return false;
            }
            return true;
        }

        public void ModifyTransaction(IClosable window)
        {
            ModifyTransaction();
            window.Close();
        }

        public void ModifyTransaction()
        {
            using (var db = new DataModel())
            {
                CurrentTransaction.CategoryID = SelectedCategory.CategoryID;
                Transactions tmp = db.Transactions.First(t => t.TransactionID == CurrentTransaction.TransactionID);
                db.Entry(tmp).CurrentValues.SetValues(CurrentTransaction);
                db.SaveChanges();
                SelectedTransaction.Copy(tmp);
            }
        }
    }
}
