using System.Windows.Controls;
using System.Windows.Input;
using PFM.ViewModels;

namespace PFM
{
    /// <summary>
    /// Interaction logic for SignUpPage.xaml
    /// </summary>
    public partial class SignUpPage : Page
    {
        public SignUpPage()
        {
            InitializeComponent();
            LastNameTB.Focus();
            Keyboard.Focus(LastNameTB);
            DataContext = new SignUpViewModel(this);
        }

        public string getPassword()
        {
            return PasswordTB.Password;
        }

        public string getChkPassword()
        {
            return PasswordChkTB.Password;
        }

        public void clearPasswords()
        {
            PasswordTB.Password = "";
            PasswordChkTB.Password = "";
            PasswordTB.Focus();
            Keyboard.Focus(PasswordTB);
        }
    }
}
