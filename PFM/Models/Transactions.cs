namespace PFM.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    /// <summary>
    /// Class for transactions
    /// </summary>
    public partial class Transactions
    {
        /// <summary>
        /// The identifier of the transaction
        /// </summary>
        [Key]
        public int TransactionID { get; set; }

        /// <summary>
        /// The identifier of the category which the transacion belongs to
        /// </summary>
        public int CategoryID { get; set; }

        /// <summary>
        /// The amount of the transaction, always positive
        /// </summary>
        [Column(TypeName = "money")]
        public decimal Amount { get; set; }

        /// <summary>
        /// The date of the transaction
        /// </summary>
        [Column(TypeName = "date")]
        public DateTime TransactionDate { get; set; }


        /// <summary>
        /// Comment for the transaction
        /// </summary>
        [StringLength(100)]
        public string Comment { get; set; }


        /// <summary>
        /// The connecting account
        /// </summary>
        public int AccountID { get; set; }

        /// <summary>
        /// The date when the transaction was created in application
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// The current balance of the account after the transaction, it is not stored in database
        /// </summary>
        [NotMapped]
        public decimal CurrentBalance { get; set; }

        /// <summary>
        /// The connecting account
        /// </summary>
        public virtual Accounts Accounts { get; set; }

        /// <summary>
        /// The connecting category
        /// </summary>
        public virtual Categories Categories { get; set; }
    }
}
