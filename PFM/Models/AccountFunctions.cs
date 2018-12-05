using System.Linq;

namespace PFM.Models
{

    public partial class Accounts
    {
        public override string ToString()
        {
            return AccountName;
        }

        public static Accounts GetDefaultAccount(Users user)
        {
            using (var db = new DataModel())
            {
                return db.Accounts.FirstOrDefault(a => (a.UserID == user.UserID) && a.Default);
            }
        }

        public bool IsModified()
        {
            using (var db = new DataModel())
            {
                var refAccount = db.Accounts.First(a => a.AccountID == AccountID);
                if (AccountName != refAccount.AccountName || Currency != refAccount.Currency ||
                Default != refAccount.Default || StartBalance != refAccount.StartBalance)
                    return true;
                return false;
            }
        }
    }
}
