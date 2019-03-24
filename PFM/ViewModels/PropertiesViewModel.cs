using PFM.Commands;
using System.Windows.Input;

namespace PFM.ViewModels
{
    class PropertiesViewModel : BaseViewModel
    {
        public MainViewModel MainViewModel { get; set; }

        public ICommand ModifyCommand { get; set; }
        
        public PropertiesViewModel(MainViewModel mainViewModel)
        {
            MainViewModel = mainViewModel;
            Initialize();
        }

        public void Initialize()
        {
            Name = "Beállítások";

            ModifyCommand = new RelayCommand(
                    p => Modify(),
                    p => CanModify());
        }

        public bool CanModify()
        {
            return true;
        }

        public void Modify()
        {

        }
    }
}
