using System.Windows.Input;

namespace PFM.ViewModels
{
    class WindowViewModel : BaseViewModel
    {
        public PageList ActualPage { get; set; } = PageList.Login;
        public ICommand RegisterCommand { get; set; }
        
        public WindowViewModel()
        {
            //RegisterCommand = new RegisterCommand(this);
        }
    }

    /// <summary>
    /// Pages to navigate
    /// </summary>
    public enum PageList
    {
        /// <summary>
        /// Login page
        /// </summary>
        Login = 0,
        /// <summary>
        /// Signup page
        /// </summary>
        SignUp = 1
    }
}
