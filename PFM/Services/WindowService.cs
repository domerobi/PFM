using Microsoft.Win32;
using PFM.ViewModels;
using PFM.Views;
using System.Windows;

namespace PFM.Services
{
    class WindowService : IWindowService
    {
        public void OpenModifyTransactionWindow(ModifyTransactionViewModel viewModel)
        {
            ModifyTransactionView view = new ModifyTransactionView
            {
                DataContext = viewModel
            };

            view.Show();
        }

        public void CloseWindow(IClosable window)
        {
            window.Close();
        }

        public bool ConfirmDelete()
        {
            var res = MessageBox.Show("Biztosan törölni szeretné a kijelölt tranzakciót?", "Tranzakció törlése",
                                     MessageBoxButton.YesNo, MessageBoxImage.Asterisk);

            return res == MessageBoxResult.Yes ? true : false;
        }

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

        public void OpenCreateAccountWindow(AccountViewModel viewModel)
        {
            CreateAccountView view = new CreateAccountView()
            {
                DataContext = viewModel
            };

            view.ShowDialog();
        }

        public void UserMessage(string message)
        {
            MessageBox.Show(message);
        }
    }
}
