using System;
using MemoSoftV3.Models;
using Prism.Commands;
using Prism.Services.Dialogs;

namespace MemoSoftV3.ViewModels
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class CommentEditPageViewModel : IDialogAware
    {
        public event Action<IDialogResult> RequestClose;

        public string Title => string.Empty;

        public Comment Comment { get; set; }

        public DatabaseManager DatabaseManager { get; private set; }
        
        public DelegateCommand CloseCommand => new (() =>
        {
            RequestClose?.Invoke(new DialogResult());
        });

        public DelegateCommand ToggleCheckableCommand => new (() =>
        {
            DatabaseManager.Add(
                new DatabaseAction
                {
                    DateTime = DateTime.Now,
                    Kind = Comment.IsCheckable ? Kind.ToCheckable : Kind.ToUnCheckable,
                    Target = Target.Comment,
                    TargetId = Comment.Id,
                });
        });

        public DelegateCommand CheckCommand => new (() =>
        {
            DatabaseManager.Add(
                new DatabaseAction
                {
                    DateTime = DateTime.Now,
                    Kind = Comment.Checked ? Kind.Check : Kind.UnCheck,
                    Target = Target.Comment,
                    TargetId = Comment.Id,
                });
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
            Comment = parameters.GetValue<Comment>(nameof(Comment));
            DatabaseManager = parameters.GetValue<DatabaseManager>(nameof(DatabaseManager));
        }
    }
}