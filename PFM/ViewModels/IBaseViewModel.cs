using PFM.Models;

namespace PFM.ViewModels
{
    interface IBaseViewModel
    {
        string Name { get; set; }
        Accounts CurrentAccount { get; set; }
        Users CurrentUser { get; set; }
        bool Selected { get; set; }
    }
}
