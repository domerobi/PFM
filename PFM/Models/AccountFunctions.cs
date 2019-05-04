using System.Linq;

namespace PFM.Models
{

    public partial class Accounts
    {
        /// <summary>
        /// Base ToString overrided to give back the name of the account
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return AccountName;
        }

        /// <summary>
        /// Gives back the default account for a specified user
        /// </summary>
        /// <param name="user">The user, whose default account needs to be returned</param>
        /// <returns></returns>
        public static Accounts GetDefaultAccount(Users user)
        {
            using (var db = new DataModel())
            {
                return db.Accounts.FirstOrDefault(a => (a.UserID == user.UserID) && a.Default);
            }
        }

        /// <summary>
        /// Decides if the current account differs from its last version in the database
        /// </summary>
        /// <returns></returns>
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
