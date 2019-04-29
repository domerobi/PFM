using System;

namespace PFM.Services
{
    public class CommonFunctionService
    {
        /// <summary>
        /// Calculates the difference between two dates and returns this in months
        /// </summary>
        /// <param name="date1">Date to subtract from</param>
        /// <param name="date2">Date to subtract</param>
        /// <returns></returns>
        public static int MonthDifference(DateTime date1, DateTime date2)
        {
            return ((date1.Year - date2.Year) * 12) +
                date1.Month - date2.Month;
        }
    }
}
