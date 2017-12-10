using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace PFM
{
    /// <summary>
    /// ViewModel for representing items from database
    /// </summary>
    class InventoryViewModel : BaseViewModel
    {
        #region Attributes

        public ObservableCollection<Inventory> InventoryRecords { get;set; }
        public ItemTypeViewModel ItemType { get; set; }
        public SearchViewModel SearchItem { get; set; }
        public ICommand AddCommand { get; set; }
        public ICommand ImportCommand { get; set; }
        public ICommand SearchCommand { get; set; }
        public ICommand ResetCommand { get; set; }
        public SqlConnection Con { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public InventoryViewModel()
        {
            // Set connection to the database
            Con = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=PFMDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            ItemType = new ItemTypeViewModel();
            SearchItem = new SearchViewModel();

            // Get records from the database
            ReadFromDB();

            // set command
            AddCommand = new AddItemCommand(this);
            ImportCommand = new ImportCommand(this);
            SearchCommand = new SearchCommand(this);
            ResetCommand = new ResetCommand(this);
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
