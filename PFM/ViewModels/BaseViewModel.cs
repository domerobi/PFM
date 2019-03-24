using PropertyChanged;
using System.ComponentModel;
using PFM.Models;

namespace PFM.ViewModels
{
    class BaseViewModel : INotifyPropertyChanged, IBaseViewModel
    {
        public string Name { get; set; }
        public Accounts CurrentAccount { get; set; }
        public Users CurrentUser { get; set; }
        public bool Selected { get; set; }

        /// <summary>
        /// Public event to be able to raise PropertyChanged on child viewmodel's properties
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        
    }
}
