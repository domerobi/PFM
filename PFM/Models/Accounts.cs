namespace PFM
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Accounts
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Accounts()
        {
            Transactions = new HashSet<Transactions>();
            Transactions1 = new HashSet<Transactions>();
        }

        [Key]
        public int AccountID { get; set; }

        public int UserID { get; set; }

        public int Outdated { get; set; }

        public int Balance { get; set; }

        [Required]
        [StringLength(3)]
        public string Currency { get; set; }

        [Required]
        [StringLength(50)]
        public string AccountName { get; set; }

        public DateTime LastModify { get; set; }

        public DateTime CreateDate { get; set; }

        public virtual Users Users { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Transactions> Transactions { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Transactions> Transactions1 { get; set; }
    }
}
