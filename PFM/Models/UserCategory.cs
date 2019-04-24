using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PFM.Models
{
    public partial class UserCategory
    {
        [Key, Column(Order = 0)]
        public int UserID { get; set; }

        [Key, Column(Order = 1)]
        public int CategoryID { get; set; }

        public virtual Users User { get; set; }
        public virtual Categories Category { get; set; }

        /// <summary>
        /// If set to true, this category will not be optimalized by the calculation
        /// </summary>
        public bool ExcludeFromCalculation { get; set; }

        /// <summary>
        /// Sets the optimalization priority for the category. The higher the priority the later it will be optimalized
        /// </summary>
        public int  Priority { get; set; }

        /// <summary>
        /// The maximum percentage of the monthly amount average, which can be reduced by calculation
        /// </summary>
        public double  Limit { get; set; }

        /// <summary>
        /// The timestamp of the creation
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// Time of the last modification to this record
        /// </summary>
        public DateTime LastModify { get; set; }

    }
}
