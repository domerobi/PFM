using PFM.Models;

namespace PFM.ViewModels
{
    class PropertiesViewModel : BaseViewModel
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        
        public PropertiesViewModel()
        {
            DataModel dataModel = new DataModel();
             
        }
    }
}
