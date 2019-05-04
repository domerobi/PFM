namespace PFM.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    /// <summary>
    /// The exhange rates with valid date intervals
    /// </summary>
    [Table("ExchangeRateHist")]
    public partial class ExchangeRateHist
    {
        /// <summary>
        /// The currency to exchange to
        /// </summary>
        [Key]
        [Column(Order = 0)]
        [StringLength(3)]
        public string Currency { get; set; }


        /// <summary>
        /// The currency to exchange from
        /// </summary>
        [Required]
        [StringLength(3)]
        public string BaseCurrency { get; set; }

        /// <summary>
        /// The rate between the two currency
        /// </summary>
        public decimal Rate { get; set; }

        /// <summary>
        /// The date from the rate is valid
        /// </summary>
        [Key]
        [Column(Order = 1, TypeName = "date")]
        public DateTime ValidFrom { get; set; }

        /// <summary>
        /// The date until the rate is valid
        /// </summary>
        [Column(TypeName = "date")]
        public DateTime? ValidTo { get; set; }
    }
}
