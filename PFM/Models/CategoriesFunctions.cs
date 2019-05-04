using System.Data.Entity;
using System.Linq;

namespace PFM.Models
{
 
    public partial class Categories
    {
        /// <summary>
        /// Base ToString overrided to give back the name of the category
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.CategoryName;
        }

        /// <summary>
        /// Copy only the attributes from an other category
        /// </summary>
        /// <param name="original">The category to copy from</param>
        public void Copy(Categories original)
        {
            CategoryID = original.CategoryID;
            CategoryName = original.CategoryName;
            CategoryDirectionID = original.CategoryDirectionID;
            CategoryDirections.Copy(original.CategoryDirections);
            Default = original.Default;
        }

        /// <summary>
        /// Create a category
        /// </summary>
        /// <param name="categoryDirection">The direction of the category to be created</param>
        /// <returns></returns>
        public bool Create(CategoryDirections categoryDirection)
        {
            CategoryDirectionID = categoryDirection.DirectionID;
            Default = false;
            CategoryDirections = null;

            using (var db = new DataModel())
            {
                db.Categories.Add(this);
                bool success = db.SaveChanges() > 0;
                if (success)
                {
                    CategoryDirections = categoryDirection;
                }
                return success;
            }
        }
    }
}
