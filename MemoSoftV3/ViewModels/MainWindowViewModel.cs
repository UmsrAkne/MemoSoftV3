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

        public MainWindowViewModel()
        {
            LoadCommand.Execute();
        }

        public string Title { get => title; set => SetProperty(ref title, value); }

        public ObservableCollection<Comment> Comments { get; private set; }

        private DatabaseManager DatabaseManager { get; set; }

        private DelegateCommand LoadCommand => new (() =>
        {
            if (DatabaseManager == null)
            {
                var dbContext = new DatabaseContext();
                dbContext.Database.EnsureCreated();
                DatabaseManager = new DatabaseManager(dbContext);
            }

            Comments = new ObservableCollection<Comment>(DatabaseManager.SearchComments(new SearchOption()));
        });
    }
}