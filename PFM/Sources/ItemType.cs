using System.Collections.Generic;

namespace PFM
{
    /// <summary>
    /// Type of the item, which stores its category list also
    /// </summary>
    class ItemType
    {
        public string Type { get; set; }

        public IList<ItemCategory> Categories { get; set; }

        public override string ToString()
        {
            return this.Type;
        }
    }
}
