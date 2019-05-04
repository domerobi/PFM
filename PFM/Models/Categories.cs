namespace PFM.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    /// <summary>
    /// Class for categories that connect to transactions
    /// </summary>
    public partial class Categories
    {
        /// <summary>
        /// Initilaize collections
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Categories()
        {
            Transactions = new HashSet<Transactions>();
            UserCategories = new HashSet<UserCategory>();
        }

        /// <summary>
        /// The identifier of the category
        /// </summary>
        [Key]
        public int CategoryID { get; set; }

        /// <summary>
        /// The identifier of the connecting direction
        /// </summary>
        public int CategoryDirectionID { get; set; }

        /// <summary>
        /// The name of the category
        /// </summary>
        [Required]
        [StringLength(50)]
        public string CategoryName { get; set; }

        /// <summary>
        /// Defines if this category is created automatically for a new user
        /// </summary>
        public bool Default { get; set; }

        /// <summary>
        /// The connecting direction
        /// </summary>
        public virtual CategoryDirections CategoryDirections { get; set; }

        /// <summary>
        /// Transactions which are in this category
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Transactions> Transactions { get; set; }

        /// <summary>
        /// A collection of users, who are using this category through a cross referenc table
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserCategory> UserCategories { get; set; }
    }
}
