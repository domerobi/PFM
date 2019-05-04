using System;

namespace PFM.Services
{
    /// <summary>
    /// Methods which are used in common scenarios
    /// </summary>
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

        /// <summary>
        /// Calculates the difference between two dates and returns this in days
        /// </summary>
        /// <param name="date1">Date to count from</param>
        /// <param name="date2">Date to count to</param>
        /// <returns></returns>
        public static int DayDifference(DateTime date1, DateTime date2)
        {
            int difference = 0;
            for (DateTime i = new DateTime(date1.Year, date1.Month, date1.Day); i < date2; i = i.AddDays(1))
            {
                difference++;
            }
            return difference;
        }
    }
}
