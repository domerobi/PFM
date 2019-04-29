using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PFM.Views
{
    /// <summary>
    /// Interaction logic for ReportBalanceControl.xaml
    /// </summary>
    public partial class ReportBalanceControl : UserControl
    {
        public ReportBalanceControl()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            App.Current.Dispatcher.Invoke(() =>
            {
                if(DataContext is ViewModels.BalanceViewModel)
                {
                    ViewModels.BalanceViewModel balanceViewModel = DataContext as ViewModels.BalanceViewModel;
                    balanceViewModel.SetLineChart();
                }
                //if (DataContext is ViewModels.CalculationViewModel)
                //{
                //    ViewModels.CalculationViewModel calcViewModel = DataContext as ViewModels.CalculationViewModel;
                //    calcViewModel.
                //}
            });
        }
    }
}
