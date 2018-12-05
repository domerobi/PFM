using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;

namespace PFM.Pages
{
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        ViewModels.LoginViewModel viewModel;

        public LoginPage(string username = null)
        {
            /*
            Thread.CurrentThread.CurrentCulture = new CultureInfo("hu-HU");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("hu-HU");
            LanguageProperty.OverrideMetadata(typeof(FrameworkElement), new FrameworkPropertyMetadata(
                        XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));
            */        
            InitializeComponent();
            if(username != null)
            {
                UserNameTB.Text = username;
                PasswordTB.Focus();
                Keyboard.Focus(PasswordTB);
            }
            else
            {
                UserNameTB.Focus();
                Keyboard.Focus(UserNameTB);
            }
            viewModel = FindResource("viewModel") as ViewModels.LoginViewModel;
            viewModel.SetLoginPage(this);
            //DataContext = new LoginViewModel(this);
        }

        public string getPassword()
        {
            return PasswordTB.Password;
        }

        public void clearPassword()
        {
            PasswordTB.Password = "";
        }

        public void clearUserName()
        {
            UserNameTB.Text = "";
            UserNameTB.Focus();
            Keyboard.Focus(UserNameTB);
        }
    }
}
