using System.ComponentModel;

namespace PFM.Models
{
    public partial class Transactions : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

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
