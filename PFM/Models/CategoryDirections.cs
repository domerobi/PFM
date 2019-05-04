namespace PFM.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    /// <summary>
    /// The direction of a category
    /// </summary>
    public partial class CategoryDirections
    {
        /// <summary>
        /// Initialize connecting categories collection
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CategoryDirections()
        {
            Categories = new HashSet<Categories>();
        }

        /// <summary>
        /// The identifier of the directions
        /// </summary>
        [Key]
        public int DirectionID { get; set; }

        /// <summary>
        /// The name of the direction
        /// </summary>
        [StringLength(20)]
        public string DirectionName { get; set; }

        /// <summary>
        /// The categories which belongs to this direction
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Categories> Categories { get; set; }
    }
}
