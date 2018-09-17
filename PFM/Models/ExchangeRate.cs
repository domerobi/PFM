namespace PFM
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ExchangeRate")]
    public partial class ExchangeRate
    {
        [Key]
        [StringLength(3)]
        public string Currency { get; set; }

        [Required]
        [StringLength(3)]
        public string BaseCurrency { get; set; }

        public decimal Rate { get; set; }
    }
}
