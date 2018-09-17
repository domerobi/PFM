using System.Data.SqlClient;

namespace PFM
{
    class PropertiesViewModel : BaseViewModel
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }

        public IDataBase DataBase { get; set; }

        public PropertiesViewModel()
        {
            DataBase = new CloudDatabase();
            DataModel dataModel = new DataModel();
             
        }
    }
}
