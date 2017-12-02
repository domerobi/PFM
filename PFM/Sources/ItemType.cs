using System.Collections.Generic;

namespace PFM
{
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
