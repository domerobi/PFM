using LiveCharts;
using System;

namespace PFM
{
    /// <summary>
    /// ViewModel for representing data on a chartesian chart
    /// </summary>
    public class ChartesianChartViewModel
    {
        public Func<double, string> Formatter { get; set; }
        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        
    }
}