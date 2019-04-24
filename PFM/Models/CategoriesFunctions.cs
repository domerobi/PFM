using System.Data.Entity;
using System.Linq;

namespace PFM.Models
{
 
    public partial class Categories
    {
        public override string ToString()
        {
            return this.CategoryName;
        }

        public void Copy(Categories original)
        {
            CategoryID = original.CategoryID;
            CategoryName = original.CategoryName;
            CategoryDirectionID = original.CategoryDirectionID;
            CategoryDirections.Copy(original.CategoryDirections);
            Default = original.Default;
        }

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
