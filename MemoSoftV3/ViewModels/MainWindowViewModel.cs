using System.Collections.ObjectModel;
using System.Linq;
using MemoSoftV3.Models;
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
                            DatabaseManager.GetComments(new SearchOption(option)));
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

        public SearchOption GroupSearchOption { get; } = new ();

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
            var pageName = $"{entity.GetType().Name}EditPage";
            var paramName = entity.GetType().Name;
            
            // ReSharper disable once UseObjectOrCollectionInitializer
            // コーディング規約を優先するため、あえてオブジェクト初期化子を使用していない。
            var param = new DialogParameters();
            param.Add(paramName, entity);
            param.Add(nameof(DatabaseManager), DatabaseManager);

            dialogService.ShowDialog(pageName, param, _ =>
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

        public DelegateCommand<Comment> ChangeFavoriteCommand => new ((comment) =>
        {
            DatabaseManager.Add(new DatabaseAction(comment, comment.IsFavorite ? Kind.ToFavorite : Kind.UnFavorite));
            LoadCommand.Execute();
        });

        public DatabaseManager DatabaseManager { get; }

        public DelegateCommand LoadCommand => new (() =>
        {
            Groups = new ObservableCollection<Group>(DatabaseManager.GetGroups(GroupSearchOption));
            Tags = new ObservableCollection<Tag>(DatabaseManager.GetTags(new SearchOption()));

            // (CurrentGroup not null) and (IsSmartGroup is true)
            var cms = CurrentGroup is { IsSmartGroup: true, }
                ? DatabaseManager.GetComments(new SearchOption())
                : DatabaseManager.GetComments(searchOption);

            Comments = new ObservableCollection<Comment>(cms);
        });
    }
}