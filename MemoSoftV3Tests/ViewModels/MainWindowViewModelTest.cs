using MemoSoftV3.Models;
using MemoSoftV3.ViewModels;
using MemoSoftV3Tests.Models;
using Prism.Services.Dialogs;

namespace MemoSoftV3Tests.ViewModels
{
    public class MainWindowViewModelTest
    {
        private MainWindowViewModel MainWindowViewModel
        {
            get
            {
                var vm = new MainWindowViewModel(new DialogServiceMock(), new DatabaseMock());

                var manager = vm.DatabaseManager;
                manager.Add(new Group { Name = "defaultGroup", });
                manager.Add(new Group { Name = "otherGroup", });
                manager.Add(new Comment { GroupId = 1, Text = "testComment1", });
                manager.Add(new Comment { GroupId = 2, Text = "nextComment2", });
                manager.Add(new Comment { GroupId = 2, Text = "nextComment3", });
                manager.Add(new Comment { GroupId = 2, Text = "nextComment4", });
                manager.Add(new Tag { Name = "testTagA", });
                manager.Add(new Tag { Name = "testTagB", });
                manager.Add(new Tag { Name = "testTagC", });
                manager.Add(new TagMap { TagId = 1, CommentId = 1, });
                manager.Add(new TagMap { TagId = 2, CommentId = 1, });
                manager.Add(new TagMap { TagId = 1, CommentId = 2, });
                manager.Add(new SubComment { Text = "subCommentA", ParentCommentId = 1, TimeTracking = true, });
                manager.Add(new SubComment { Text = "subCommentB", ParentCommentId = 1, TimeTracking = true, });
                manager.Add(new SubComment { Text = "subCommentC", ParentCommentId = 2, });

                vm.LoadCommand.Execute();
                return vm;
            }
        }

        [Test]
        public void 通常生成テスト()
        {
            var _ = MainWindowViewModel;
        }

        [Test]
        public void AddSubCommentCommandTest()
        {
            var vm = MainWindowViewModel;
            var selectedComment = vm.Comments[0];
            selectedComment.ChildSubComment = new SubComment { Text = "newSubComment1", };
            vm.AddSubCommentCommand.Execute(selectedComment);

            selectedComment.ChildSubComment = new SubComment { Text = "newSubComment2", };
            vm.AddSubCommentCommand.Execute(selectedComment);

            Assert.Multiple(() =>
            {
                Assert.That(selectedComment.SubComments[2].Text, Is.EqualTo("newSubComment1"));
                Assert.That(selectedComment.SubComments[3].Text, Is.EqualTo("newSubComment2"));
            });
        }

        [Test]
        public void ChangeFavoriteCommandTest()
        {
            var vm = MainWindowViewModel;
            var sampleComment = vm.Comments.OrderBy(c => c.Id).First();

            var k01 = vm.DatabaseManager.GetDatabaseActions().FirstOrDefault(a => a.Kind == Kind.ToFavorite);
            var k02 = vm.DatabaseManager.GetDatabaseActions().FirstOrDefault(a => a.Kind == Kind.UnFavorite);
            Assert.IsNull(k01, "現時点で該当アクションはデータベースに無い。");
            Assert.IsNull(k02, "現時点で該当アクションはデータベースに無い。");

            // コマンドを実行した結果、データベースに DatabaseAction が記録されるかを確認する。

            sampleComment.IsFavorite = true;
            vm.ChangeFavoriteCommand.Execute(sampleComment);
            var k = vm.DatabaseManager.GetDatabaseActions().First(a => a.Kind == Kind.ToFavorite);
            Assert.That(k.TargetId, Is.EqualTo(1), "コマンド実行後は、お気に入りを変更したという情報が記録されている");

            sampleComment.IsFavorite = false;
            vm.ChangeFavoriteCommand.Execute(sampleComment);
            var k2 = vm.DatabaseManager.GetDatabaseActions().First(a => a.Kind == Kind.UnFavorite);
            Assert.That(k2.TargetId, Is.EqualTo(1), "コマンド実行後は、お気に入りを変更したという情報が記録されている");
        }
    }

    internal class DialogServiceMock : IDialogService
    {
        public void Show(string name, IDialogParameters parameters, Action<IDialogResult> callback)
        {
        }

        public void Show(string name, IDialogParameters parameters, Action<IDialogResult> callback, string windowName)
        {
        }

        public void ShowDialog(string name, IDialogParameters parameters, Action<IDialogResult> callback)
        {
        }

        public void ShowDialog(string name, IDialogParameters parameters, Action<IDialogResult> callback,
            string windowName)
        {
        }
    }
}