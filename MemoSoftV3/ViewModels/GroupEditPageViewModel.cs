using System;
using System.Linq;
using MemoSoftV3.Models;
using Prism.Commands;
using Prism.Services.Dialogs;

namespace MemoSoftV3.ViewModels
{
    // ReSharper disable once ClassNeverInstantiated.Global
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

        public DelegateCommand SaveChangesCommand => new (() =>
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
            DatabaseManager = parameters.GetValue<DatabaseManager>(nameof(DatabaseManager));
            CurrentGroup = parameters.GetValue<Group>(nameof(Group));
            CurrentGroup.CanChangeToSmartGroup = DatabaseManager
                .GetComments(new SearchOption())
                .All(c => c.GroupId != CurrentGroup.Id);
        }
    }
}