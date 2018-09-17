namespace PFM
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ExchangeRateHist")]
    public partial class ExchangeRateHist
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(3)]
        public string Currency { get; set; }

        [Required]
        [StringLength(3)]
        public string BaseCurrency { get; set; }

        public decimal Rate { get; set; }

        [Key]
        [Column(Order = 1, TypeName = "date")]
        public DateTime ValidFrom { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ValidTo { get; set; }
    }
}
