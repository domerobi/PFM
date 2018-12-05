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

    public enum PageList
    {
        /// <summary>
        /// Login page
        /// </summary>
        Login = 0,
        SignUp = 1,
        Reports = 2,
        NewAccount = 3,
        Properties = 4,
        Transactions = 5
    }
}
