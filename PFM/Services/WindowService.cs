using Microsoft.Win32;
using PFM.ViewModels;
using PFM.Views;
using System.Windows;

namespace PFM.Services
{
    /// <summary>
    /// Service for doing interactive tasks
    /// </summary>
    class WindowService : IWindowService
    {
        /// <summary>
        /// Opening new window to modify the selected transaction
        /// </summary>
        /// <param name="viewModel">The view model for the window</param>
        public void OpenModifyTransactionWindow(ModifyTransactionViewModel viewModel)
        {
            ModifyTransactionView view = new ModifyTransactionView
            {
                DataContext = viewModel
            };

            view.ShowDialog();
        }

        /// <summary>
        /// Close the given window
        /// </summary>
        /// <param name="window">Window to close</param>
        public void CloseWindow(IClosable window)
        {
            window.Close();
        }

        /// <summary>
        /// Confirm window for deleting the selected transaction
        /// </summary>
        /// <returns></returns>
        public bool ConfirmDelete()
        {
            var res = MessageBox.Show("Biztosan törölni szeretné a kijelölt tranzakciót?", "Tranzakció törlése",
                                     MessageBoxButton.YesNo, MessageBoxImage.Asterisk);

            return res == MessageBoxResult.Yes ? true : false;
        }

        /// <summary>
        /// Opens the file select dialog, and returns the selected file's path
        /// </summary>
        /// <returns></returns>
        public string GetImportFileName()
        {
            OpenFileDialog openfile = new OpenFileDialog();
            openfile.DefaultExt = ".xlsx";
            openfile.Filter = "(.xlsx)|*.xlsx";
            string filePath = "";

            var browsefile = openfile.ShowDialog();

            if (browsefile == true)
            {
                filePath = openfile.FileName;
            }

            return filePath;
        }

        /// <summary>
        /// Opens a new window to create a new account
        /// </summary>
        /// <param name="viewModel">View model for the window</param>
        public void OpenCreateAccountWindow(AccountViewModel viewModel)
        {
            CreateAccountView view = new CreateAccountView()
            {
                DataContext = viewModel
            };

            view.ShowDialog();
        }

        /// <summary>
        /// Opens a messae box and displays the given message
        /// </summary>
        /// <param name="message">The message to be displayed</param>
        public void UserMessage(string message)
        {
            MessageBox.Show(message);
        }

        /// <summary>
        /// Opens a new window to create a new category or modify the selected
        /// </summary>
        /// <param name="viewModel">View model for the window</param>
        public void OpenCategoryWindow(CategoryViewModel viewModel)
        {
            CategoryView view = new CategoryView()
            {
                DataContext = viewModel
            };

            view.ShowDialog();
        }

        /// <summary>
        /// New messagebox with the given question and title
        /// </summary>
        /// <param name="question">Question to be asked</param>
        /// <param name="title">Title of the message box</param>
        /// <returns></returns>
        public bool UserQuestion(string question, string title)
        {
            var res = MessageBox.Show(question, "Tranzakció törlése",
                                     MessageBoxButton.YesNo, MessageBoxImage.Asterisk);

            return res == MessageBoxResult.Yes ? true : false;
        }
    }
}
