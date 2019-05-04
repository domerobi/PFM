using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel;

namespace PFM.Models
{
    public partial class UserCategory : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Copy the values from the parameter object to self
        /// </summary>
        /// <param name="original">The original object to copy from</param>
        public void Copy(UserCategory original)
        {
            UserID = original.UserID;
            CategoryID = original.CategoryID;
            User = original.User;
            Category = original.Category;
            ExcludeFromCalculation = original.ExcludeFromCalculation;
            Priority = original.Priority;
            Limit = original.Limit;
            CreateDate = original.CreateDate;
            LastModify = original.LastModify;
        }

        /// <summary>
        /// Check if the current category is modified by reading and comparing the last saved version from database
        /// </summary>
        /// <returns></returns>
        public bool IsModified()
        {
            using (var db = new DataModel())
            {
                // read the current usercategory with the primary key from the database
                var tmp = db.UserCategory
                            .Include(uc => uc.Category)
                            .Include(c => c.Category.CategoryDirections)
                            .First(uc => uc.UserID == UserID && uc.CategoryID == CategoryID);

                // check if current category is differ from the one read from database
                if (IsModified(tmp))
                    return true;
                return false;
            }
        }

        /// <summary>
        /// Check if the current category is modified by comparing it with the given original category
        /// </summary>
        /// <param name="original">The original category, which need to be compared with self</param>
        /// <returns></returns>
        public bool IsModified(UserCategory original)
        {
            // compare the properties
            if (original.Category.CategoryName != Category.CategoryName ||
                original.Category.CategoryDirections.DirectionID != Category.CategoryDirections.DirectionID ||
                original.ExcludeFromCalculation != ExcludeFromCalculation ||
                original.Priority != Priority ||
                original.Limit != Limit)
                return true;
            return false;
        }

        /// <summary>
        /// Check if current category's name differ from the one stored in database
        /// </summary>
        /// <returns></returns>
        public bool IsNameModified()
        {
            using (var db = new DataModel())
            {
                var tmp = db.UserCategory
                            .Include(uc => uc.Category)
                            .First(uc => uc.UserID == UserID && uc.CategoryID == CategoryID);
                return tmp.Category.CategoryName != Category.CategoryName;
            }
        }


        /// <summary>
        /// Create a cross reference record between user and category
        /// </summary>
        /// <param name="user">The connecting user</param>
        /// <param name="category">The connecting category</param>
        /// <returns></returns>
        public bool Create(Users user, Categories category)
        {
            CreateDate = DateTime.Now;
            LastModify = CreateDate;
            UserID = user.UserID;
            CategoryID = category.CategoryID;
            User = null;
            Category = null;

            using (var db = new DataModel())
            {
                db.UserCategory.Add(this);
                bool success = db.SaveChanges() > 0;
                if (success)
                {
                    User = user;
                    Category = category;
                }

                return success;
            }
        }

    }
}
