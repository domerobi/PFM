namespace PFM
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Inventory")]
    public partial class Inventory
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Type { get; set; }

        [Required]
        [StringLength(50)]
        public string Category { get; set; }

        public int Sum { get; set; }

        [Column(TypeName = "date")]
        public DateTime Date { get; set; }

        [StringLength(200)]
        public string Comment { get; set; }
    }
}
