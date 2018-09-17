namespace PFM
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Transactions
    {
        [Key]
        public int TransactionID { get; set; }

        public int CategoryID { get; set; }

        [Column(TypeName = "money")]
        public decimal Amount { get; set; }

        [Column(TypeName = "date")]
        public DateTime TransactionDate { get; set; }

        public int AccountFrom { get; set; }

        public int AccountTo { get; set; }

        public virtual Accounts Accounts { get; set; }

        public virtual Accounts Accounts1 { get; set; }

        public virtual Categories Categories { get; set; }
    }
}
