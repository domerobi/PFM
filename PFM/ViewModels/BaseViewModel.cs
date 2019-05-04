using PropertyChanged;
using System.ComponentModel;
using PFM.Models;

namespace PFM.ViewModels
{
    /// <summary>
    /// Base view model for all view models in the application
    /// </summary>
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
