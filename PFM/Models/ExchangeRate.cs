namespace PFM.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    /// <summary>
    /// The exchange rate of a currency
    /// </summary>
    [Table("ExchangeRate")]
    public partial class ExchangeRate
    {
        /// <summary>
        /// The currency to exchange to
        /// </summary>
        [Key]
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
    }
}
