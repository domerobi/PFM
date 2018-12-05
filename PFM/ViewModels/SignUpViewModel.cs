using System.Data;
using System.Windows.Input;
using System.Linq;
using PFM.Models;

namespace PFM.ViewModels
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

        
        public SignUpViewModel(SignUpPage signUpPage)
        {
            //LoginCommand = new LoginCommand(this);
            mPage = signUpPage;
            SignUpPage = (SignUpPage)mPage;
            ActualPage = PageList.SignUp;
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
            using (var dataModel = new DataModel())
            {
                var query = from user in dataModel.Users
                            where user.Username == UserName
                            select user;
                if (query.Count() > 0)
                    return true;
                return false;

            }
        }

        public int Register(Users user)
        {
            // Add actual user to the database
            using (var dataModel = new DataModel())
            {
                dataModel.Users.Add(user);
                return dataModel.SaveChanges();
            }
        }
    }
}
