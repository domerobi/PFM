namespace PFM.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class CalculationData
    {

        [Key]
        public int CalculationDataID { get; set; }

        public int CalculationID { get; set; }

        public int CategoryID { get; set; }

        [Column(TypeName = "money")]
        public decimal Average { get; set; }

        [Column(TypeName = "money")]
        public decimal Limit { get; set; }

        public virtual Calculation Calculation { get; set; }

        public virtual Categories Categories { get; set; }
    }
}
