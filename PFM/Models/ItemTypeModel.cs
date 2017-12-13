using System;
using System.Collections.Generic;

namespace PFM
{
    /// <summary>
    /// Type of the item, which stores its category list also
    /// </summary>
    class ItemTypeModel
    {
        public string Type { get; set; }

        public IList<String> Categories { get; set; }

        /// <summary>
        /// Helps to convert an ItemType to a string value to show it on the screen
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.Type;
        }
    }
}
