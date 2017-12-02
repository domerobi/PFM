using PropertyChanged;
using System.ComponentModel;

namespace PFM
{
    class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
