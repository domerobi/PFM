using LiveCharts;
using LiveCharts.Wpf;
using System.Collections.Generic;

namespace PFM.ViewModels
{
    /// <summary>
    /// ViewModel for representing data on a pie chart
    /// </summary>
    class CategoryChartViewModel : BaseViewModel, IChartModel
    {
        public SeriesCollection PieSeries { get; set; }
        public ICollection<Series> Series { get; set; }
        public string Title { get; set; }

        public CategoryChartViewModel()
        {
            PieSeries = new SeriesCollection();
            Series = new List<Series>();
        }

        public void Refresh()
        {
            PieSeries = new SeriesCollection();
            PieSeries.AddRange(Series);
        }
    }
}
