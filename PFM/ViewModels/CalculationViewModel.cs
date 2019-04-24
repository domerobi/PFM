using PFM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PFM.ViewModels
{
    /// <summary>
    /// The view model representation of Calculation and CalculationData database classes
    /// </summary>
    class CalculationViewModel : BaseViewModel
    {
        public MainViewModel MainViewModel { get; set; }

        #region Filters

        public Calculation CalculationFilter { get; set; }

        public bool SaveCalculation { get; set; }

        #endregion

        /// <summary>
        /// Constructor of the class
        /// </summary>
        /// <param name="mainViewModel">The MainViewModel which holds the logged in user and the selected account</param>
        public CalculationViewModel(MainViewModel mainViewModel)
        {
            MainViewModel = mainViewModel;
            Initialize();
        }

        /// <summary>
        /// Initialize the viewmodel when it is created
        /// </summary>
        public void Initialize()
        {
            Name = "Kalkuláció készítése";
            Reset();
        }

        /// <summary>
        /// Resets the filter fields with following default values:
        ///   - Due             : Today + 6 months
        ///   - Amount          : 0
        ///   - CalculationName : ""
        ///   - SaveCalculation : false
        /// </summary>
        private void Reset()
        {
            CalculationFilter = new Calculation
            {
                DueDate = DateTime.Today.AddMonths(6),
                Amount = 0,
                CalculationName = ""
            };
            SaveCalculation = false;
        }
    }
}
