namespace PFM.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// Calculation data for a calculation for one category
    /// </summary>
    public partial class CalculationData
    {
        /// <summary>
        /// Primary key for the class
        /// </summary>
        [Key]
        public int CalculationDataID { get; set; }

        /// <summary>
        /// Foreign key for the head record
        /// </summary>
        public int CalculationID { get; set; }

        /// <summary>
        /// Foreign key for the category
        /// </summary>
        public int CategoryID { get; set; }

        /// <summary>
        /// The average expense for the current category in the last six months
        /// </summary>
        [Column(TypeName = "money")]
        public decimal Average { get; set; }

        /// <summary>
        /// The maximum amount to spend on the current category until the due date
        /// </summary>
        [Column(TypeName = "money")]
        public decimal Limit { get; set; }

        /// <summary>
        /// Navigation property to Calculation
        /// </summary>
        public virtual Calculation Calculation { get; set; }

        /// <summary>
        /// Navigation property to Category
        /// </summary>
        public virtual Categories Categories { get; set; }
    }
}
