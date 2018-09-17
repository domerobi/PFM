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
using System.Windows.Shapes;

namespace PFM
{
    /// <summary>
    /// Interaction logic for CreateAccountView.xaml
    /// </summary>
    public partial class CreateAccountView : Window
    {
        public CreateAccountView(Users user)
        {
            InitializeComponent();

            AccountViewModel viewModel = new AccountViewModel(user);
            viewModel.CloseAction = new Action(() => this.Close());
            this.DataContext = viewModel;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Ha kilép, akkor nem jön létre a számla. Biztosan ezt akarja?", "Képernyő elhagyása mentés nélkül", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if(result == MessageBoxResult.Yes)
                this.Close();
        }
    }
}
