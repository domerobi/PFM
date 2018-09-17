using System.Windows.Input;

namespace PFM
{
    class WindowViewModel : BaseViewModel
    {
        public Pages ActualPage { get; set; } = Pages.Login;
        public ICommand RegisterCommand { get; set; }
        
        public WindowViewModel()
        {
            //RegisterCommand = new RegisterCommand(this);
        }
    }

    public enum Pages
    {
        /// <summary>
        /// Login page
        /// </summary>
        Login = 0,
        SignUp = 1,
        Reports = 2,
        NewAccount = 3,
        Properties = 4
    }
}
