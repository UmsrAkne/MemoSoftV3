using System;
using Prism.Commands;
using Prism.Services.Dialogs;

namespace MemoSoftV3.ViewModels
{
    public class GroupEditPageViewModel : IDialogAware
    {
        public event Action<IDialogResult> RequestClose;

        public string Title => string.Empty;

        public DelegateCommand CloseCommand => new (() =>
        {
            RequestClose?.Invoke(new DialogResult());
        });

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
        }
    }
}