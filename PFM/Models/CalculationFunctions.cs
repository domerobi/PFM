using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PFM.Models
{
    public partial class Calculation
    {
        /// <summary>
        /// Check if calculation with a given name already exists
        /// </summary>
        /// <param name="CalculationName">Calculation name to check</param>
        /// <returns></returns>
        public bool CheckCalculationName(string CalculationName)
        {
            using (var db = new DataModel())
            {
                return db.Calculation.Count(c => c.CalculationName == CalculationName) == 0;
            }
        }
    }
}
