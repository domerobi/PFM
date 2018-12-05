using PFM.ViewModels;
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
    /// Interaction logic for ReportViewControl.xaml
    /// </summary>
    public partial class ReportViewControl : UserControl
    {
        public ReportViewControl()
        {
            InitializeComponent();
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            App.Current.Dispatcher.Invoke(() =>
            {
                ReportViewModel reportViewModel = DataContext as ReportViewModel;
                if(reportViewModel != null)
                    reportViewModel.RefreshCharts();
            });
        }
    }
}
