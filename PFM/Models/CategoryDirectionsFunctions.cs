namespace PFM.Models
{

    public partial class CategoryDirections
    {
        public override string ToString()
        {
            return this.DirectionName;
        }

        public void Copy(CategoryDirections original)
        {
            DirectionID = original.DirectionID;
            DirectionName = original.DirectionName;
        }
    }
}
