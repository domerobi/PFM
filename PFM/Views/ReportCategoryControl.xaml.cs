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
    /// Interaction logic for ReportCategoryControl.xaml
    /// </summary>
    public partial class ReportCategoryControl : UserControl
    {
        public ReportCategoryControl()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModels.CategoryViewModel categoryViewModel = DataContext as ViewModels.CategoryViewModel;
            categoryViewModel.SetCategoryPieCharts();

            App.Current.Dispatcher.Invoke(() =>
            {

            });
        }
    }
}
