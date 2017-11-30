using System.Collections.Generic;

namespace PFM
{
    class ItemType
    {
        public string Type { get; set; }

        public ICollection<ItemCategory> Categories { get; set; }
    }
}
