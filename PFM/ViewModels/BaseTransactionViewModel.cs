using System.Linq;
using System.Data.Entity;
using PFM.Models;
using System.Collections.ObjectModel;

namespace PFM.ViewModels
{
    /// <summary>
    /// Base view model for transaction view models
    /// </summary>
    class BaseTransactionViewModel : BaseViewModel
    {
        #region protected Attributes
        
        protected CategoryDirections selectedCategoryDirections;
        
        #endregion

        #region public Attributes

        public ObservableCollection<CategoryDirections> CategoryDirections { get; set; }
        public Categories SelectedCategory { get; set; }
        public CategoryDirections SelectedCategoryDirection
        {
            get { return selectedCategoryDirections; }
            set
            {
                selectedCategoryDirections = value;
                SelectedCategory = SelectedCategoryDirection.Categories.Where(c => c.CategoryID < 1).First();
            }
        }

        #endregion

        #region public Methods

        /// <summary>
        /// Initializes the categories
        /// </summary>
        public void InitializeCategories()
        {
            using (var db = new DataModel())
            {
                db.CategoryDirections.Include(c => c.Categories).Load();
                CategoryDirections = new ObservableCollection<CategoryDirections>(db.CategoryDirections.Local.OrderBy(o => o.DirectionName));

                CategoryDirections SelectCD = new CategoryDirections()
                {
                    DirectionID = 0,
                    DirectionName = "Válassz...",
                    Categories = new ObservableCollection<Categories>
                    {
                        new Categories
                        {
                            CategoryID = 0,
                            CategoryName = "Válassz..."
                        }
                    }
                };

                int SelectCategoryID = -1;
                foreach (var dir in CategoryDirections)
                {
                    dir.Categories = new ObservableCollection<Categories>(dir.Categories.OrderBy(o => o.CategoryName));
                    dir.Categories.Add(new Categories
                    {
                        CategoryID = SelectCategoryID,
                        CategoryName = "Válassz..."
                    });
                    SelectCategoryID--;
                }
                CategoryDirections.Add(SelectCD);

                SelectedCategoryDirection = CategoryDirections.Where(cd => cd.DirectionID == 0).First();
            }
        
        }

        #endregion
    }
}
