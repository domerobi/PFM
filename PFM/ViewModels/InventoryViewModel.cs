using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace PFM
{
    /// <summary>
    /// ViewModel for representing items from database
    /// </summary>
    class InventoryViewModel : BaseViewModel
    {
        public List<Inventory> InventoryRecords { get;set; }

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="con">Connection to the database</param>
        public InventoryViewModel(SqlConnection con)
        {
            this.InventoryRecords = new List<Inventory>();
            // Get records from the database
            con.Open();
            PFMDBEntities dataEntities = new PFMDBEntities();
            var query =
                from item in dataEntities.Inventory
                    //where item.Category == "Utazás"
                orderby item.Date descending
                select new { item.Id, item.Date, item.Type, item.Category, item.Sum, item.Comment };
            // Store the records in a list
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

        #endregion
        
    }
}
