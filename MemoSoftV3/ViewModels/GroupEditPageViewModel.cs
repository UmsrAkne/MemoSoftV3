using System;
using MemoSoftV3.Models;
using Prism.Commands;
using Prism.Services.Dialogs;

namespace MemoSoftV3.ViewModels
{
    public class GroupEditPageViewModel : IDialogAware
    {
        public event Action<IDialogResult> RequestClose;

        public Group CurrentGroup { get; private set; }

        public DelegateCommand CloseCommand => new (() =>
        {
            RequestClose?.Invoke(new DialogResult());
        });

        public DelegateCommand ToggleSmartGroupCommand => new (() =>
        {
            DatabaseManager.SaveChanges();
        });

        public DatabaseManager DatabaseManager { get; private set; }

        public string Title => string.Empty;

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            CurrentGroup = parameters.GetValue<Group>(nameof(Group));
            DatabaseManager = parameters.GetValue<DatabaseManager>(nameof(DatabaseManager));
        }
    }
}