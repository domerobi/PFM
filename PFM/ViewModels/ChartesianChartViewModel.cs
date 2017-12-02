using LiveCharts;
using System;

namespace PFM
{
    public class ChartesianChartViewModel
    {
        public Func<double, string> Formatter { get; set; }
        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        
    }
}