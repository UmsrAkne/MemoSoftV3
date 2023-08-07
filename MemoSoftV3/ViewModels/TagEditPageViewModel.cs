using System;
using MemoSoftV3.Models;
using Prism.Commands;
using Prism.Services.Dialogs;

namespace MemoSoftV3.ViewModels
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class TagEditPageViewModel : IDialogAware
    {
        private string originalName = string.Empty;

        public event Action<IDialogResult> RequestClose;

        public string Title => string.Empty;

        public Tag CurrentTag { get; set; }

        public DelegateCommand SaveAndCloseCommand => new (() =>
        {
            RequestClose?.Invoke(new DialogResult(ButtonResult.Yes));
        });

        public DelegateCommand CancelCommand => new (() =>
        {
            if (CurrentTag.Name != originalName)
            {
                CurrentTag.Name = originalName;
            }

            RequestClose?.Invoke(new DialogResult(ButtonResult.Cancel));
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
            CurrentTag = parameters.GetValue<Tag>(nameof(Tag));
            originalName = CurrentTag.Name;
        }
    }
}