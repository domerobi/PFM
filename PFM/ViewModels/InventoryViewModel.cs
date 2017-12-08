using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
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
        public ICommand AddCommand { get; set; }
        private SqlConnection Con { get; set; }

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

            // Get records from the database
            ReadFromDB(Con);

            // set command
            AddCommand = new AddItemCommand(this);
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

        /// <summary>
        /// Initialize item collection from the database
        /// </summary>
        /// <param name="con">database connection</param>
        public void ReadFromDB(SqlConnection con)
        {
            // create a new collection
            this.InventoryRecords = new ObservableCollection<Inventory>();
            // connect to the database
            con.Open();
            PFMDBEntities dataEntities = new PFMDBEntities();
            // select all items by date
            var query =
                from item in dataEntities.Inventory
                orderby item.Date descending
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
            
            con.Close();
        }

        /// <summary>
        /// Add currently created item to the database
        /// </summary>
        public void AddToDB()
        {
            // create sql connection
            SqlConnection con = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=PFMDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            con.Open();

            // create Inventory item to add
            Inventory Item = ItemType.CreateItem();

            // build insert command
            SqlCommand cmd = con.CreateCommand();
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
            con.Close();
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
