namespace PFM.Models
{

    public partial class CategoryDirections
    {
        /// <summary>
        /// Base ToString overrided to give back the name of the direction
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.DirectionName;
        }

        /// <summary>
        /// Copy only the attributes from an other direction
        /// </summary>
        /// <param name="original">The direction to copy from</param>
        public void Copy(CategoryDirections original)
        {
            DirectionID = original.DirectionID;
            DirectionName = original.DirectionName;
        }
    }
}
