namespace PFM.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class Calculation
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Calculation()
        {
            CalculationData = new HashSet<CalculationData>();
        }

        [Key]
        public int CalculationID { get; set; }

        public int AccountID { get; set; }

        /// <summary>
        /// The name of the calculation for future display
        /// </summary>
        [StringLength(40)]
        public string CalculationName { get; set; }

        /// <summary>
        /// The goal amount which is needed on the due date
        /// </summary>
        [Column(TypeName = "money")]
        public decimal Amount { get; set; }

        /// <summary>
        /// The date when the goal amount is needed
        /// </summary>
        public DateTime DueDate { get; set; }

        /// <summary>
        /// The create date of the calculation
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// It controls if actual balance is included in the calculation or the goal amount need to be achieved only by savings
        /// </summary>
        public bool BalanceIncluded { get; set; }

        public virtual Accounts Accounts { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CalculationData> CalculationData { get; set; }

    }
}
