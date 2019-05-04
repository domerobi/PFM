namespace PFM.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    /// <summary>
    /// The users of the application
    /// </summary>
    public partial class Users
    {
        /// <summary>
        /// Initialize the collections
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Users()
        {
            Accounts = new HashSet<Accounts>();
            UserCategories = new HashSet<UserCategory>();
        }

        /// <summary>
        /// Identifier of the user
        /// </summary>
        [Key]
        public int UserID { get; set; }

        /// <summary>
        /// Defines the lifecycle of the user
        /// </summary>
        public int Outdated { get; set; }

        /// <summary>
        /// Username of the user
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Username { get; set; }

        /// <summary>
        /// First name of the user
        /// </summary>
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        /// <summary>
        /// Last name of the user
        /// </summary>
        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        /// <summary>
        /// Email of the user
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Email { get; set; }

        /// <summary>
        /// Hash of the password
        /// </summary>
        [Required]
        [StringLength(64)]
        public string Password { get; set; }

        /// <summary>
        /// Create date of the user
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// Accounts of the user
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Accounts> Accounts { get; set; }

        /// <summary>
        /// Categories of the user
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserCategory> UserCategories { get; set; }
    }
}
