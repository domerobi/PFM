using LiveCharts;
using LiveCharts.Wpf;

namespace PFM
{
    /// <summary>
    /// ViewModel for representing data on a pie chart
    /// </summary>
    class CategoryChartViewModel : BaseViewModel
    {
        public SeriesCollection PieSeries { get; set; }
    }
}
