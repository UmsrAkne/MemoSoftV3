using System.Collections.ObjectModel;
using System.Linq;
using MemoSoftV3.Models;
using MemoSoftV3.Views;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;

namespace MemoSoftV3.ViewModels
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class MainWindowViewModel : BindableBase
    {
        private readonly IDialogService dialogService;
        private readonly SearchOption searchOption = new ();
        private string title = "Prism Application";
        private string commandText = string.Empty;
        private ObservableCollection<Comment> comments;
        private ObservableCollection<Group> groups;
        private Group currentGroup;
        private Tag currentTag;
        private ObservableCollection<Tag> tags = new ();

        public MainWindowViewModel(IDialogService dialogService, IDataSource dataSource)
        {
            DatabaseManager = new DatabaseManager(dataSource);

            if (DatabaseManager.GetGroups(new SearchOption()).Count == 0)
            {
                DatabaseManager.Add(new Group { Name = "All", IsSmartGroup = true, });
            }
            
            this.dialogService = dialogService;
            LoadCommand.Execute();
        }

        public string Title { get => title; set => SetProperty(ref title, value); }

        public string CommandText { get => commandText; set => SetProperty(ref commandText, value); }

        public Group CurrentGroup
        {
            get => currentGroup;
            set
            {
                if (value != null && !string.IsNullOrEmpty(value.Name))
                {
                    if (value.IsSmartGroup)
                    {
                        var parser = new CliParser();
                        var option = parser.ParseSearchCommand(value.Command);

                        Comments = new ObservableCollection<Comment>(
                            DatabaseManager.SearchComments(new SearchOption(option)));
                    }
                }

                SetProperty(ref currentGroup, value);
            }
        }

        public Tag CurrentTag { get => currentTag; set => SetProperty(ref currentTag, value); }

        public ObservableCollection<Comment> Comments
        {
            get => comments;
            private set => SetProperty(ref comments, value);
        }

        public ObservableCollection<Group> Groups { get => groups; private set => SetProperty(ref groups, value); }

        public ObservableCollection<Tag> Tags { get => tags; set => SetProperty(ref tags, value); }

        public DelegateCommand CommandExecutionCommand => new (() =>
        {
            var parser = new CliParser();
            var cmWa = parser.GetCommentWithoutArgs(CommandText);
            var option = parser.Parse(parser.GetArgsString(CommandText));
            DatabaseManager.ExecuteCli(cmWa, option);
            
            CommandText = string.Empty;
            LoadCommand.Execute();
        });

        public DelegateCommand<IDatabaseEntity> ShowEditPageCommand => new (entity =>
        {
            var pageName = string.Empty;
            var paramName = string.Empty;

            switch (entity)
            {
                case Group group:
                    group.CanChangeToSmartGroup =
                        DatabaseManager.SearchComments(new SearchOption())
                            .All(c => c.GroupId != group.Id);

                    pageName = nameof(GroupEditPage);
                    paramName = nameof(Group);
                    break;

                case Tag:
                    pageName = nameof(TagEditPage);
                    paramName = nameof(Tag);
                    break;
            }

            dialogService.ShowDialog(
                pageName, new DialogParameters { { paramName, entity }, }, _ =>
                {
                });
        });

        public DelegateCommand ChangeSearchConditionsCommand => new (() =>
        {
            searchOption.GroupName = CurrentGroup == null ? string.Empty : CurrentGroup.Name;
            searchOption.TagTexts = Tags.Where(t => t.Applying).Select(t => t.Name).ToList();
            LoadCommand.Execute();
        });

        public DelegateCommand<Comment> AddSubCommentCommand => new (comment =>
        {
            comment.ChildSubComment.ParentCommentId = comment.Id;
            DatabaseManager.Add(comment.ChildSubComment);
            comment.ChildSubComment = new SubComment();
            LoadCommand.Execute();
        });

        public DelegateCommand<SubComment> ReloadSubCommentTimeTrackingCommand => new (subComment =>
        {
            DatabaseManager.Add(new DatabaseAction(
                subComment, subComment.TimeTracking ? Kind.SetTimeTracking : Kind.CancelTimeTracking));
            
            var cm = Comments.Single(c => c.Id == subComment.ParentCommentId);
            DatabaseManager.ReloadSubCommentTimeTracking(cm.SubComments);
        });

        private DatabaseManager DatabaseManager { get; set; }

        private DelegateCommand LoadCommand => new (() =>
        {
            Groups = new ObservableCollection<Group>(DatabaseManager.GetGroups(new SearchOption()));
            Tags = new ObservableCollection<Tag>(DatabaseManager.GetTags(new SearchOption()));

            // (CurrentGroup not null) and (IsSmartGroup is true)
            var cms = CurrentGroup is { IsSmartGroup: true, }
                ? DatabaseManager.SearchComments(new SearchOption())
                : DatabaseManager.SearchComments(searchOption);

            Comments = new ObservableCollection<Comment>(DatabaseManager.InjectCommentProperties(cms));
        });
    }
}