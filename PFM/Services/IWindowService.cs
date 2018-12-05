using PFM.ViewModels;

namespace PFM.Services
{
    interface IWindowService
    {
        void OpenModifyTransactionWindow(ModifyTransactionViewModel viewModel);
        void OpenCreateAccountWindow(AccountViewModel viewModel);
        void CloseWindow(IClosable window);
        bool ConfirmDelete();
        string GetImportFileName();
        void UserMessage(string message);
    }
}
