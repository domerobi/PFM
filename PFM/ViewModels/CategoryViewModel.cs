using System;
using System.Linq;
using System.Data.Entity;
using PFM.Models;
using System.Globalization;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using PFM.Services;
using PFM.Commands;

namespace PFM.ViewModels
{
    class CategoryViewModel : BaseViewModel
    {

        #region Private properties

        IWindowService windowService;

        #endregion

        #region Public properties

        /// <summary>
        /// The main viewmodel of the program, that handles all of the globally needed attributes and other viewmodels
        /// </summary>
        public MainViewModel MainViewModel { get; set; }

        /// <summary>
        /// The collection of the categories for the logged in user
        /// </summary>
        public ObservableCollection<UserCategory> UserCategories { get; set; }

        /// <summary>
        /// It holds the selected category in the list
        /// </summary>
        public UserCategory SelectedCategory { get; set; }

        /// <summary>
        /// For creating and modifying categories we need a list of categorydirections, from which one can be bound for the category
        /// </summary>
        public ObservableCollection<CategoryDirections> CategoryDirections { get; set; }

        /// <summary>
        /// True if the category was modified successfully
        /// </summary>
        public bool IsModified { get; set; }

        /// <summary>
        /// Collection of priority levels, from 1 to 10
        /// </summary>
        public ObservableCollection<int> PriorityLevels
        {
            get
            {
                return new ObservableCollection<int>(Enumerable.Range(1,10).ToList());
            }
        }

        public ICommand AddCategoryCommand { get; set; }
        public ICommand DeleteCategoryCommand { get; set; }
        public ICommand ModifyCategoryCommand { get; set; }
        public ICommand OKCommand { get; set; }
        public ICommand CloseCommand { get; set; }
        
        #endregion

        /// <summary>
        /// Defines the behaviour of the viewmodel.
        /// </summary>
        public enum InteractionMode
        {
            Menu,
            Modify,
            Create
        }

        /// <summary>
        /// Constructror for the categoryviewmodel
        /// </summary>
        /// <param name="mainViewModel">The main viewmodel of the application</param>
        /// <param name="interactionMode">Defines how should the viewmodel behave</param>
        /// <param name="userCategory">Only used when interactionMode = Modify, it holds the selected category</param>
        public CategoryViewModel(MainViewModel mainViewModel, InteractionMode interactionMode, UserCategory userCategory = null)
        {
            MainViewModel = mainViewModel;
            Initialize(interactionMode, userCategory);    
        }

        /// <summary>
        /// Initialize the viewmodel when it is created.
        /// </summary>
        private void Initialize(InteractionMode interactionMode, UserCategory userCategory)
        {
            switch (interactionMode)
            {
                case InteractionMode.Menu:
                    Name = "Kategóriák karbantartása";
                    break;
                case InteractionMode.Modify:
                    Name = "Kategória módosítása";
                    SelectedCategory = new UserCategory();
                    SelectedCategory.Copy(userCategory);
                    OKCommand = new RelayCommand(
                            p => Modify(p as IClosable),
                            p => IsAttributesFilled());
                    break;
                case InteractionMode.Create:
                    Name = "Kategória létrehozása";
                    SelectedCategory = new UserCategory()
                    {
                        Category = new Categories()
                        {
                            CategoryDirections = new CategoryDirections()
                        },
                        Priority = 1
                    };
                    OKCommand = new RelayCommand(
                            p => Create(p as IClosable),
                            p => IsAttributesFilled());
                    break;
                default:
                    break;
            }

            IsModified = false;
            
            windowService = new WindowService();

            AddCategoryCommand = new RelayCommand(
                    p => CreateInteractive());

            CloseCommand = new RelayCommand(
                    p => windowService.CloseWindow(p as IClosable));

            ModifyCategoryCommand = new RelayCommand(
                    p => ModifyInteractive(),
                    p => IsSelected(true));

            DeleteCategoryCommand = new RelayCommand(
                    p => Delete(),
                    p => IsSelected(false));

            // load the categories for the logged in user
            using (var db = new DataModel())
            {
                UserCategories = new ObservableCollection<UserCategory>(db
                                                                        .UserCategory
                                                                        .Include(c => c.Category)
                                                                        .Include(c => c.Category.CategoryDirections)
                                                                        .Include(c => c.User)
                                                                        .Where( uc => uc.UserID == MainViewModel.CurrentUser.UserID)
                                                                        .ToList());

                CategoryDirections = new ObservableCollection<CategoryDirections>(db
                                                                                  .CategoryDirections
                                                                                  .OrderBy(cd => cd.DirectionName)
                                                                                  .ToList());
                if (interactionMode == InteractionMode.Create)
                {
                    var SelectCD = new CategoryDirections()
                    {
                        DirectionID = 0,
                        DirectionName = "Válassz..."
                    };
                    CategoryDirections.Add(SelectCD);
                    SelectedCategory.Category.CategoryDirections = CategoryDirections.First(cd => cd.DirectionID == 0);
                }

                if (interactionMode == InteractionMode.Modify)
                {
                    // select the categorydirection from the collection
                    SelectedCategory.Category.CategoryDirections = CategoryDirections
                                                                     .First(cd => cd.DirectionID == SelectedCategory.Category.CategoryDirectionID);
                }

            }
        }

        private void Delete()
        {
            if (windowService.UserQuestion("Biztosan törölni szeretné a kijelölt kategóriát?", "Kategória törlése"))
            {
                using (var db = new DataModel())
                {
                    var deletedUserCategory = db.UserCategory.Find(SelectedCategory.UserID, SelectedCategory.CategoryID);
                    db.UserCategory.Remove(deletedUserCategory);
                    db.SaveChanges();
                    UserCategories.Remove(SelectedCategory);
                    SelectedCategory = null;
                }
            }
        }

        /// <summary>
        /// Check if mandatory attributes are filled.
        /// </summary>
        /// <returns>Returns true, if category can be created.</returns>
        public bool IsAttributesFilled()
        {
            if (String.IsNullOrEmpty(SelectedCategory.Category.CategoryName) || SelectedCategory.Category.CategoryDirections.DirectionID == 0 ||
                (!SelectedCategory.ExcludeFromCalculation && (SelectedCategory.Limit < 0 || SelectedCategory.Limit > 100 || SelectedCategory.Priority < 0)))
                return false;
            
            return true;
        }

        /// <summary>
        /// Creating category from user input.
        /// </summary>
        /// <returns>Returns true, if the new category is saved to the database.</returns>
        public bool CreateInteractive()
        {
            var newCategory = new CategoryViewModel(MainViewModel, InteractionMode.Create);
            windowService.OpenCategoryWindow(newCategory);
            using (var db = new DataModel())
            {
                var chkCategory = db.Categories.Find(newCategory.SelectedCategory.CategoryID);
                if (chkCategory != null)
                {
                    UserCategories.Add(newCategory.SelectedCategory);
                    SelectedCategory = UserCategories.First(uc => uc.CategoryID == newCategory.SelectedCategory.CategoryID);
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// Creates new category for the logged in user.
        /// </summary>
        /// <returns>Returns true, if the new category is saved to the database.</returns>
        public bool Create()
        {

            //using (var db = new DataModel())
            //{
            //    //var aaa = new Categories
            //    //{
            //    //    CategoryDirectionID = 1,
            //    //    CategoryName = "proba",
            //    //    Default = false
            //    //};
            //    // first create the category

            //    if (db.SaveChanges() <= 0)
            //        return false;

            //    var newCategory = db.Categories.First(c => c.CategoryName == SelectedCategory.Category.CategoryName);

            //    newCategory.UserCategories.Add(SelectedCategory);
            //    SelectedCategory.User.UserCategories.Add(SelectedCategory);
            //    //db.Categories.Attach(newCategory);
            //    //db.Entry<Users>(SelectedCategory.User).State = EntityState.Modified;
            //    //db.Entry<CategoryDirections>(SelectedCategory.Category.CategoryDirections).State = EntityState.Modified;
            //    //db.UserCategory.Add(SelectedCategory);
            //    return db.SaveChanges() > 0;
            //}

            SelectedCategory.Category.Create(SelectedCategory.Category.CategoryDirections);

            return SelectedCategory.Create(MainViewModel.CurrentUser, SelectedCategory.Category);
        }

        public void Create(IClosable window)
        {
            if (CategoryExists(SelectedCategory.Category.CategoryName))
            {
                windowService.UserMessage("Létezik már ilyen névvel kategória, kérem adjon meg másik nevet!");
                return;
            }
            if (Create())
                windowService.UserMessage("A kategória sikeresen létrejött!");
            else
                windowService.UserMessage("A kategória létrehozása sikertelen!");
            window.Close();
        }

        
        /// <summary>
        /// Modify the selected category in a new window
        /// </summary>
        public void ModifyInteractive()
        {
            // create a new viewmodel with the selected category
            var modifyCategory = new CategoryViewModel(MainViewModel, InteractionMode.Modify, SelectedCategory);
            
            // open the window to modify
            windowService.OpenCategoryWindow(modifyCategory);

            // copy the value back, if it was modified
            if(modifyCategory.IsModified)
                SelectedCategory.Copy(modifyCategory.SelectedCategory);
        }

        // Modify the selected category, and save it to the database
        public bool Modify()
        {
            SelectedCategory.LastModify = DateTime.Now;
            using (var db = new DataModel())
            {
                // read the current category's last saved version from database
                var category = db.Categories
                                 .Find(SelectedCategory.CategoryID);

                // assign new values and save it
                category.CategoryDirectionID = SelectedCategory.Category.CategoryDirections.DirectionID;
                db.Entry(category).CurrentValues.SetValues(SelectedCategory.Category);
                db.SaveChanges();

                // read the current usercategory's last saved version from database with the modified category
                var tmp = db.UserCategory
                            .Include(uc => uc.Category)
                            .Include(uc => uc.Category.CategoryDirections)
                            .First(uc => uc.UserID == SelectedCategory.UserID && uc.CategoryID == SelectedCategory.CategoryID);

                // copy the new values
                db.Entry(tmp).CurrentValues.SetValues(SelectedCategory);

                // attach the unmodified records, and set the modified state which needs to be modified
                //db.Categories.Attach(tmp.Category);
                //db.CategoryDirections.Attach(tmp.Category.CategoryDirections);
                //db.Users.Attach(tmp.User);

                // save everything to the database
                return db.SaveChanges() > 0;
            }
        }

        /// <summary>
        /// Modify the selected category in a window
        /// </summary>
        /// <param name="window">The window where the modification take place</param>
        public void Modify(IClosable window)
        {
            // check if the categoryname was modified
            if (SelectedCategory.IsNameModified())
            {
                // make sure there is no category with the same name already in database
                if (CategoryExists(SelectedCategory.Category.CategoryName))
                {
                    windowService.UserMessage("Létezik már ilyen névvel kategória, kérem adjon meg másik nevet!");
                    return;
                }
            }

            // try to modify, if any changes was made
            if (SelectedCategory.IsModified())
            {
                if (Modify())
                {
                    windowService.UserMessage("A kategória sikeresen módosításra került!");
                    IsModified = true;
                }
                else
                {
                    windowService.UserMessage("A kategóriát nem sikerül módosítani!");
                }
            }
            else
            {
                windowService.UserMessage("Nem történt módosítás.");
            }

            // close the window anyway
            window.Close();
        }

        /// <summary>
        /// Check if there is any selected category. Needed for delete and modify.
        /// </summary>
        /// <param name="selectDefault">Defines if default can be selected</param>
        /// <returns></returns>
        public bool IsSelected(bool selectDefault)
        {
            return SelectedCategory != null && (selectDefault || (!selectDefault && SelectedCategory.Category.Default == false));
        }

        /// <summary>
        /// Check if the given name already exists in database
        /// </summary>
        /// <param name="name">Name to be checked</param>
        /// <returns></returns>
        public bool CategoryExists(string name)
        {
            using (var db = new DataModel())
            {
                return db.UserCategory.FirstOrDefault(uc => uc.User.UserID == MainViewModel.CurrentUser.UserID && String.Compare(uc.Category.CategoryName, name, true) == 0 ) != null;
            }
        }





        //public void SetCategoryPieCharts()
        //{
        //    IncomeCategories = new SeriesCollection();
        //    ExpendCategories = new SeriesCollection();
        //    using (var db = new DataModel())
        //    {
        //        // Set the time interval for the chart
        //        DateTime lastMonth = DateTime.Today.AddMonths(-1);
        //        DateTime firstDayOfLastMonth = new DateTime(lastMonth.Year, lastMonth.Month, 1);
        //        DateTime lastDayOfLastMonth = DateTime.Today.AddDays(-(DateTime.Today.Day));

        //        // Get the sum of all categories of the last month
        //        var groupbyCategories = db.Transactions
        //                                    .Include(t => t.Categories.CategoryDirections)
        //                                    .Where(t => t.TransactionDate >= firstDayOfLastMonth && 
        //                                                t.TransactionDate <= lastDayOfLastMonth  &&
        //                                                t.AccountID == MainViewModel.CurrentAccount.AccountID)
        //                                    .GroupBy(t => t.Categories.CategoryName)
        //                                    .Select(t => new
        //                                    {
        //                                        Category = t.Key,
        //                                        CategoryDirection = t.FirstOrDefault().Categories.CategoryDirections.DirectionName,
        //                                        Amount = t.Sum(c => c.Amount)
        //                                    }).ToList();


        //        foreach (var cat in groupbyCategories)
        //        {
        //            PieSeries ps = new PieSeries
        //            {
        //                Title = cat.Category,
        //                Values = new ChartValues<int> { (int)cat.Amount },
        //                DataLabels = true,
        //                LabelPoint = chartPoint =>
        //                                    string.Format("{0:C0}", chartPoint.Y)
        //            };
        //            if (cat.CategoryDirection == "Kiadás")
        //                ExpendCategories.Add(ps);
        //            if (cat.CategoryDirection == "Bevétel")
        //                IncomeCategories.Add(ps);
        //        }
        //    }
        //}

        

    }
}
