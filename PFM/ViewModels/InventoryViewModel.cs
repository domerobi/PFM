using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace PFM
{
    class InventoryViewModel : BaseViewModel
    {
        public List<Inventory> InventoryRecords { get;set; }
        public InventoryViewModel(SqlConnection con)
        {
            this.InventoryRecords = new List<Inventory>();
            con.Open();
            PFMDBEntities dataEntities = new PFMDBEntities();
            var query =
                from item in dataEntities.Inventory
                    //where item.Category == "Utazás"
                orderby item.Date descending
                select new { item.Id, item.Date, item.Type, item.Category, item.Sum, item.Comment };
            foreach(var record in query)
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
    }
}
