using System.Windows.Controls;

namespace PFM
{
    /// <summary>
    /// Interaction logic for ReportPage.xaml
    /// </summary>
    public partial class ReportPage : Page
    {
        public ReportPage()
        {
            InitializeComponent();
            DataContext = new ReportViewModel();
        }
    }
}
