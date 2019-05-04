namespace PFM.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    /// <summary>
    /// Represents the AccountBalance view in the database, which calculates the actual balance of a given account
    /// </summary>
    [Table("AccountBalance")]
    public partial class AccountBalance
    {
        /// <summary>
        /// The account which balance is needed
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int AccountID { get; set; }

        /// <summary>
        /// The actual balance for the given account
        /// </summary>
        [Column(TypeName = "money")]
        public decimal? Balance { get; set; }
    }
}
