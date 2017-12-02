namespace PFM
{
    /// <summary>
    /// Category of an item, which is connected to the item type
    /// </summary>
    class ItemCategory
    {
        public string Name { get; set; }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
