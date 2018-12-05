using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;

namespace PFM.ViewModels
{
    /// <summary>
    /// ViewModel for representing data on a chartesian chart
    /// </summary>
    class ChartesianChartViewModel : BaseViewModel, IChartModel
    {
        
        public ChartesianChartViewModel()
        {
            SeriesCollection = new SeriesCollection();
            Series = new List<Series>();
        }

        public string Title { get; set; }
        public string XTitle { get; set; }
        public string YTitle { get; set; }

        public Func<double, string> Formatter { get; set; }
        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public ICollection<Series> Series { get; set; }

        public void Refresh()
        {
            SeriesCollection = new SeriesCollection();
            SeriesCollection.AddRange(Series);
        }
    }
}