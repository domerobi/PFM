using System.ComponentModel;

namespace PFM.Models
{
    public partial class Transactions : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Copy only the attributes from an other transaction
        /// </summary>
        /// <param name="original">The transaction to copy from</param>
        public void Copy(Transactions original)
        {
            AccountID = original.AccountID;
            Amount = original.Amount;
            Categories = original.Categories;
            CategoryID = original.CategoryID;
            Comment = original.Comment;
            TransactionDate = original.TransactionDate;
            TransactionID = original.TransactionID;
            CreateDate = original.CreateDate;
        }

    }
}
