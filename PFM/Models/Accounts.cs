namespace PFM.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    /// <summary>
    /// Stores all of the infromation of an account
    /// </summary>
    public partial class Accounts
    {
        /// <summary>
        /// Initializes the transaction collection
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Accounts()
        {
            Transactions = new HashSet<Transactions>();
        }

        /// <summary>
        /// Identifier of an account
        /// </summary>
        [Key]
        public int AccountID { get; set; }

        /// <summary>
        /// Identifier of the account's user
        /// </summary>
        public int UserID { get; set; }

        /// <summary>
        /// Defines the lifecycle of the account
        /// </summary>
        public int Outdated { get; set; }

        /// <summary>
        /// The balance of the account when it is created
        /// </summary>
        [Column(TypeName = "money")]
        public decimal StartBalance { get; set; }

        /// <summary>
        /// The currency of the account
        /// </summary>
        [Required]
        [StringLength(3)]
        public string Currency { get; set; }

        /// <summary>
        /// The name of the account in the application
        /// </summary>
        [Required]
        [StringLength(50)]
        public string AccountName { get; set; }

        /// <summary>
        /// Stores the time when the account was last modified
        /// </summary>
        public DateTime LastModify { get; set; }

        /// <summary>
        /// The date when the account was created
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// Defines if this is the user's default account
        /// </summary>
        public bool Default { get; set; }

        /// <summary>
        /// The account's user
        /// </summary>
        public virtual Users Users { get; set; }

        /// <summary>
        /// The transactions of the account
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Transactions> Transactions { get; set; }
    }
}
