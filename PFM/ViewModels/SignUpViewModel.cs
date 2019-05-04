using System.Data;
using System.Windows.Input;
using System.Linq;
using PFM.Models;

namespace PFM.ViewModels
{
    /// <summary>
    /// View model for signup procedure
    /// </summary>
    class SignUpViewModel : StartUpViewModel
    {
        public ICommand BackCommand { get; set; }
        public ICommand RegisterCommand { get; set; }
        public string UserName { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }

        public SignUpPage SignUpPage { get; set; }

        
        /// <summary>
        /// Initializing properties
        /// </summary>
        /// <param name="signUpPage">Page for signup</param>
        public SignUpViewModel(SignUpPage signUpPage)
        {
            //LoginCommand = new LoginCommand(this);
            mPage = signUpPage;
            SignUpPage = (SignUpPage)mPage;
            ActualPage = PageList.SignUp;
            RegisterCommand = new RegisterCommand(this);
            BackCommand = new NavigateCommand(this);

        }

        /// <summary>
        /// Gets the password from the field on the window
        /// </summary>
        /// <returns></returns>
        public string GetPassword()
        {
            return SignUpPage.getPassword();
        }

        /// <summary>
        /// Gets the password check from the field on the window
        /// </summary>
        /// <returns></returns>
        public string GetChkPassword()
        {
            return SignUpPage.getChkPassword();
        }

        /// <summary>
        /// Clears the password field
        /// </summary>
        public void ClearPasswords()
        {
            SignUpPage.clearPasswords();
        }

        /// <summary>
        /// Decides if each property is well filled to register
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Checks if the given username exists already
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Register the user
        /// </summary>
        /// <param name="user">User to register</param>
        /// <returns></returns>
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
