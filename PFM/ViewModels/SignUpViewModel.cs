using System.Data;
using System.Data.SqlClient;
using System.Windows.Input;
using System.Linq;

namespace PFM
{
    class SignUpViewModel : StartUpViewModel
    {
        public ICommand BackCommand { get; set; }
        public ICommand RegisterCommand { get; set; }
        public string UserName { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }

        public SignUpPage SignUpPage { get; set; }

        public IDataBase DataBase { get; set; }

        public SignUpViewModel(SignUpPage signUpPage)
        {
            //LoginCommand = new LoginCommand(this);
            mPage = signUpPage;
            SignUpPage = (SignUpPage)mPage;
            ActualPage = Pages.SignUp;
            DataBase = new CloudDatabase();
            RegisterCommand = new RegisterCommand(this);
            BackCommand = new NavigateCommand(this);

        }

        public string GetPassword()
        {
            return SignUpPage.getPassword();
        }

        public string GetChkPassword()
        {
            return SignUpPage.getChkPassword();
        }

        public void ClearPasswords()
        {
            SignUpPage.clearPasswords();
        }

        public bool CanRegister()
        {
            if (UserName == "")
                return false;
            if (LastName == "")
                return false;
            if (FirstName == "")
                return false;
            if (Email == "")
                return false;
            if (GetPassword() == "")
                return false;
            if (GetChkPassword() == "")
                return false;
            return true;
        }

        public bool UserAlreadyExists()
        {
            DataBase.Open();
            DataModel dataModel = new DataModel();
            var query = from user in dataModel.Users
                        where user.Username == UserName
                        select user;
            DataBase.Close();
            if (query.Count() > 0)
                return true;
            return false;
        }

        public int Register(Users user)
        {
            int result;
            DataBase.Open();

            // build sql command
            SqlCommand command = DataBase.Connection.CreateCommand();
            command.CommandType = CommandType.Text;
            command.CommandText = "INSERT INTO dbo.Users (Username, FirstName, LastName, Email, Password)" +
                                  "VALUES(@username, @firstname, @lastname, @email, @password)";
            command.Parameters.AddWithValue("@username", user.Username);
            command.Parameters.AddWithValue("@firstname", user.FirstName);
            command.Parameters.AddWithValue("@lastname", user.LastName);
            command.Parameters.AddWithValue("@email", user.Email);
            command.Parameters.AddWithValue("@password", user.Password);

            result = command.ExecuteNonQuery();

            DataBase.Close();
            return result;
        }
    }
}
