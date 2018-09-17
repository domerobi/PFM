namespace PFM
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Currency_View
    {
        [Key]
        [StringLength(3)]
        public string Currency { get; set; }
    }
}
