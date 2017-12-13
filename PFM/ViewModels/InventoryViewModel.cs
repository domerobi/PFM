using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Input;
using PFM.Views;

namespace PFM
{
    /// <summary>
    /// ViewModel for representing items from database
    /// </summary>
    class InventoryViewModel : BaseViewModel
    {
        #region Attributes

        public ObservableCollection<Inventory> InventoryRecords { get;set; }
        public MainViewModel MainVM { get; set; }
        public ItemTypeViewModel ItemType { get; set; }
        public SearchViewModel SearchItem { get; set; }
        public ItemTypeViewModel ModifyItem { get; set; }
        public ICommand AddCommand { get; set; }
        public ICommand ImportCommand { get; set; }
        public ICommand SearchCommand { get; set; }
        public ICommand ResetCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand ModifyCommand { get; set; }
        public ICommand UpdateCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        public SqlConnection Con { get; set; }
        public Inventory SelectedItem { get; set; }
        public ModifyItemView ModView { get; set; }


        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public InventoryViewModel(MainViewModel mainVM)
        {
            // Set connection to the database
            Con = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=PFMDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            MainVM = mainVM;
            ItemType = new ItemTypeViewModel();
            SearchItem = new SearchViewModel();
            
            // Get records from the database
            ReadFromDB();

            // set command
            AddCommand = new AddItemCommand(MainVM);
            ImportCommand = new ImportCommand(MainVM);
            SearchCommand = new SearchCommand(MainVM);
            ResetCommand = new ResetCommand(MainVM);
            DeleteCommand = new DeleteCommand(MainVM);
            ModifyCommand = new ModifyCommand(this);
            UpdateCommand = new UpdateCommand(MainVM);
            CancelCommand = new CancelCommand(this);
        }

        #endregion
        
        #region Methods

        /// <summary>
        /// Check for empty values before insert item to the database
        /// </summary>
        /// <returns></returns>
        public bool CanAddItem()
        {
            if (ItemType.SelectedItemType.ToString() != "Válassz típust..." && !String.IsNullOrWhiteSpace(ItemType.SelectedItemCategory)
                && ItemType.SelectedItemCategory != "Válassz kategóriát..." && !String.IsNullOrWhiteSpace(ItemType.ItemSum))
                return true;
            return false;
        }

        public bool CanImportItem()
        {
            return true;
        }

        public bool CanDeleteItem()
        {
            if (SelectedItem != null)
                return true;
            return false;
        }

        public bool CanModifyItem()
        {
            if (SelectedItem != null)
                return true;
            return false;
        }

        /// <summary>
        /// Initialize item collection from the database
        /// </summary>
        public void ReadFromDB()
        {
            // create a new collection
            this.InventoryRecords = new ObservableCollection<Inventory>();
            // connect to the database
            Con.Open();
            PFMDBEntities dataEntities = new PFMDBEntities();
            // get the filter values from search fields
            string type = SearchItem.SelectedType == SearchItem.SearchTypes[0] ? "" : SearchItem.SelectedType.ToString();
            string category = SearchItem.SelectedCategory == SearchItem.SearchCategories[0] ? "" : SearchItem.SelectedCategory;
            DateTime startDate = Convert.ToDateTime(SearchItem.StartDate);
            DateTime endDate = Convert.ToDateTime(SearchItem.EndDate);
            // select all items by date
            var query =
                from item in dataEntities.Inventory
                where item.Date >= startDate
                    && item.Date <= endDate
                    && item.Type.Contains(type)
                    && item.Category.Contains(category)
                orderby item.Date, item.Type descending
                select new { item.Id, item.Date, item.Type, item.Category, item.Sum, item.Comment };
            // Store the records in a collection
            foreach (var record in query)
            {
                this.InventoryRecords.Add(new Inventory
                {
                    Id = record.Id,
                    Type = record.Type,
                    Category = record.Category,
                    Sum = record.Sum,
                    Date = record.Date,
                    Comment = record.Comment
                });
            }

            SortInventoryByDate();


            Con.Close();
        }

        /// <summary>
        /// Add an item to the database
        /// </summary>
        /// <param name="Item">Item to add</param>
        public void AddToDB(Inventory Item)
        {
            Con.Open();
            
            // build insert command
            SqlCommand cmd = Con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "INSERT INTO dbo.Inventory (Type, Category, Sum, Date, Comment)" +
                              "VALUES(@type, @category, @sum, @date, @comment)";

            cmd.Parameters.AddWithValue("@type", Item.Type);
            cmd.Parameters.AddWithValue("@category", Item.Category);
            cmd.Parameters.AddWithValue("@sum", Item.Sum);
            cmd.Parameters.AddWithValue("@date", Item.Date);
            cmd.Parameters.AddWithValue("@comment", Item.Comment);

            // execute command
            cmd.ExecuteNonQuery();

            // close sql connection
            Con.Close();
        }

        public void DeleteFromDB(Inventory Item)
        {
            Con.Open();

            // build insert command
            SqlCommand cmd = Con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "DELETE FROM dbo.Inventory WHERE " +
                              //"Type=@type AND Category=@category AND Sum=@sum AND Date=@date AND Comment=@comment";
                              "Id=@id";
            //cmd.Parameters.AddWithValue("@type", Item.Type);
            //cmd.Parameters.AddWithValue("@category", Item.Category);
            //cmd.Parameters.AddWithValue("@sum", Item.Sum);
            //cmd.Parameters.AddWithValue("@date", Item.Date);
            //cmd.Parameters.AddWithValue("@comment", Item.Comment);
            cmd.Parameters.AddWithValue("@id", Item.Id);

            // execute command
            cmd.ExecuteNonQuery();

            // close sql connection
            Con.Close();
        }

        public void GetNextItemID()
        {
            Con.Open();

            // build command for getting the next available id from database
            SqlCommand cmd = Con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT IDENT_CURRENT ('dbo.Inventory')";
            this.ItemType.ItemID = Convert.ToInt32(cmd.ExecuteScalar()) + 1;

            Con.Close();
            
        }

        public void ImportFromExcel()
        {
            OpenFileDialog openfile = new OpenFileDialog();
            openfile.DefaultExt = ".xlsx";
            openfile.Filter = "(.xlsx)|*.xlsx";
            string filePath;
            
            var browsefile = openfile.ShowDialog();

            if (browsefile == true)
            {
                filePath = openfile.FileName;

                Microsoft.Office.Interop.Excel.Application excelApp = new Microsoft.Office.Interop.Excel.Application();
                //Static File From Base Path...........
                //Microsoft.Office.Interop.Excel.Workbook excelBook = excelApp.Workbooks.Open(AppDomain.CurrentDomain.BaseDirectory + "TestExcel.xlsx", 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
                //Dynamic File Using Uploader...........
                Microsoft.Office.Interop.Excel.Workbook excelBook = excelApp.Workbooks.Open(filePath.ToString(), 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
                Microsoft.Office.Interop.Excel.Worksheet excelSheet = (Microsoft.Office.Interop.Excel.Worksheet)excelBook.Worksheets.get_Item(1); ;
                Microsoft.Office.Interop.Excel.Range excelRange = excelSheet.UsedRange;

                int rowCnt = 0;
                int colCnt = 0;

                for (rowCnt = 2; rowCnt <= excelRange.Rows.Count; rowCnt++)
                {
                    for (colCnt = 1; colCnt <= excelRange.Columns.Count; colCnt= colCnt+5)
                    {
                        Inventory inv = new Inventory();
                        inv.Date = DateTime.FromOADate(((excelRange.Cells[rowCnt, colCnt] as Microsoft.Office.Interop.Excel.Range).Value2));
                        inv.Type = (string)(excelRange.Cells[rowCnt, colCnt + 1] as Microsoft.Office.Interop.Excel.Range).Value2;
                        inv.Category = (string)(excelRange.Cells[rowCnt, colCnt + 2] as Microsoft.Office.Interop.Excel.Range).Value2;
                        inv.Sum = Convert.ToInt32((excelRange.Cells[rowCnt, colCnt + 3] as Microsoft.Office.Interop.Excel.Range).Value2);
                        inv.Comment = (string)(excelRange.Cells[rowCnt, colCnt + 4] as Microsoft.Office.Interop.Excel.Range).Value2;
                        this.InventoryRecords.Add(inv);

                        // Add actual item to database
                        AddToDB(inv);
                    }
                }


                excelBook.Close(true, null, null);
                excelApp.Quit();
            }
        }

        public void OpenModifyItemDialog()
        {
            ModifyItem = new ItemTypeViewModel();
            int indexOfType = ModifyItem.Types.IndexOf(ModifyItem.Types.Where(type => type.Type == SelectedItem.Type).FirstOrDefault());
            ModifyItem.ItemID = SelectedItem.Id;
            ModifyItem.SelectedItemType = ModifyItem.Types[indexOfType];
            ModifyItem.Categories = ModifyItem.SelectedItemType.Categories;
            ModifyItem.SelectedItemCategory = SelectedItem.Category;
            ModifyItem.ItemSum = SelectedItem.Sum < 0 ? (-1 * SelectedItem.Sum).ToString() : SelectedItem.Sum.ToString();
            ModifyItem.ItemDate = SelectedItem.Date.ToString();
            ModifyItem.ItemComment = SelectedItem.Comment;
            ModView = new ModifyItemView
            {
                DataContext = this
            };
            ModView.Show();
        }

        public void UpdateItem(Inventory Item)
        {
            Con.Open();

            SqlCommand cmd = Con.CreateCommand();
            cmd.CommandText = "UPDATE dbo.Inventory SET " +
                              "Type=@type, Category=@category, Sum=@sum, Date=@date, Comment=@comment " +
                              "WHERE Id=@id"  ;
            cmd.Parameters.AddWithValue("@type", Item.Type);
            cmd.Parameters.AddWithValue("@category", Item.Category);
            cmd.Parameters.AddWithValue("@sum", Item.Sum);
            cmd.Parameters.AddWithValue("@date", Item.Date);
            cmd.Parameters.AddWithValue("@comment", Item.Comment);
            cmd.Parameters.AddWithValue("@id", Item.Id);
            cmd.ExecuteNonQuery();

            Con.Close();
        }

        /// <summary>
        /// Sorting Datagrid data by date
        /// </summary>
        public void SortInventoryByDate()
        {
            InventoryRecords = new ObservableCollection<Inventory>(InventoryRecords.OrderByDescending(d => d.Date ));
        }

        #endregion
    }
}
