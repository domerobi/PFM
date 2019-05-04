using System;
using System.Linq;
using System.Data.Entity;

using PFM.Models;
using LiveCharts;
using LiveCharts.Wpf;

namespace PFM.ViewModels
{
    /// <summary>
    /// A view model for displaying balance charts
    /// </summary>
    class BalanceViewModel : BaseViewModel, IChartModel
    {

        #region Public properties

        public ChartesianChartViewModel LineChart { get; set; }
        public MainViewModel MainViewModel { get; private set; }

        public bool HasCalculation
        {
            get
            {
                return false;
            }
        }

        #endregion

        public BalanceViewModel(MainViewModel mainViewModel)
        {
            Name = "Egyenleg";

            MainViewModel = mainViewModel;

            LineChart = new ChartesianChartViewModel();
            LineChart.Title = "Egyenleg változása";
            LineChart.Formatter = value => value.ToString("C0");
        }

        public void Refresh()
        {
            return;
        }
    }
}
