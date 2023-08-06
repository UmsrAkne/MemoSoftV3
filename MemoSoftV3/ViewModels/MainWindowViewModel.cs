using System.Collections.ObjectModel;
using MemoSoftV3.Models;
using Prism.Commands;
using Prism.Mvvm;

namespace MemoSoftV3.ViewModels
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class MainWindowViewModel : BindableBase
    {
        private string title = "Prism Application";
        private string commandText = string.Empty;
        private ObservableCollection<Comment> comments;
        private ObservableCollection<Group> groups;
        private Group currentGroup;

        public MainWindowViewModel()
        {
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
                    else
                    {
                        Comments = new ObservableCollection<Comment>(
                            DatabaseManager.SearchComments(new SearchOption
                            {
                                GroupName = value.Name,
                            }));
                    }
                }

                SetProperty(ref currentGroup, value);
            }
        }

        public ObservableCollection<Comment> Comments
        {
            get => comments;
            private set => SetProperty(ref comments, value);
        }

        public ObservableCollection<Group> Groups { get => groups; private set => SetProperty(ref groups, value); }

        public DelegateCommand CommandExecutionCommand => new (() =>
        {
            var parser = new CliParser();
            var cmWa = parser.GetCommentWithoutArgs(CommandText);
            var option = parser.Parse(parser.GetArgsString(CommandText));
            DatabaseManager.ExecuteCli(cmWa, option);
            
            CommandText = string.Empty;
            LoadCommand.Execute();
        });

        private DatabaseManager DatabaseManager { get; set; }

        private DelegateCommand LoadCommand => new (() =>
        {
            if (DatabaseManager == null)
            {
                var dbContext = new DatabaseContext();
                dbContext.Database.EnsureCreated();
                DatabaseManager = new DatabaseManager(dbContext);

                if (DatabaseManager.GetGroups(new SearchOption()).Count == 0)
                {
                    DatabaseManager.Add(new Group { Name = "All", IsSmartGroup = true, });
                }
            }

            Groups = new ObservableCollection<Group>(DatabaseManager.GetGroups(new SearchOption()));
            Comments = new ObservableCollection<Comment>(DatabaseManager.SearchComments(new SearchOption()));
        });
    }
}