using System.Windows;
using PFM.ViewModels;
using PFM.Commands;

namespace PFM.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindowView : Window
    {
        MainViewModel viewModel;
        
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public MainWindowView(int userID)
        {
            InitializeComponent();
            viewModel = FindResource("mainViewModel") as MainViewModel;
            viewModel.SetUser(userID);
        }

        #endregion
        
    }
    
}
