using System.Windows;
using PFM.Services;

namespace PFM.Views
{
    /// <summary>
    /// Interaction logic for CreateAccountView.xaml
    /// </summary>
    public partial class CreateAccountView : Window, IClosable
    {
        ViewModels.AccountViewModel viewModel;

        public CreateAccountView()
        {
            InitializeComponent();
        }
        
        
    }
}
