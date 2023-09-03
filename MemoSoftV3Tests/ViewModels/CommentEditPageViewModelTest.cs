using MemoSoftV3.Models;
using MemoSoftV3.ViewModels;
using MemoSoftV3Tests.Models;
using Prism.Services.Dialogs;

namespace MemoSoftV3Tests.ViewModels
{
    [TestFixture]
    public class CommentEditPageViewModelTest
    {
        [Test]
        public void ToggleCheckableCommandTest()
        {
            var vm = GetViewModel();
            var actDefault = vm.DatabaseManager
                .GetDatabaseActions()
                .FirstOrDefault(a => a.Kind is Kind.ToCheckable or Kind.ToUnCheckable);

            Assert.Multiple(() =>
            {
                Assert.That(actDefault, Is.Null, "初期状態ではまだ null のはず");
                Assert.That(vm.Comment.IsCheckable, Is.False);
            });

            vm.Comment.IsCheckable = true;
            vm.ToggleCheckableCommand.Execute();

            var actAfter = vm.DatabaseManager
                .GetDatabaseActions()
                .Where(a => a.Kind is Kind.ToCheckable or Kind.ToUnCheckable)
                .ToList()[0];

            Assert.That(actAfter.Kind, Is.EqualTo(Kind.ToCheckable), "チェック可能に切り替えたので、 ToCheckable が入っている");

            vm.Comment.IsCheckable = false;
            vm.ToggleCheckableCommand.Execute();

            var actSecond = vm.DatabaseManager
                .GetDatabaseActions()
                .Where(a => a.Kind is Kind.ToCheckable or Kind.ToUnCheckable)
                .ToList()[1];

            Assert.That(actSecond.Kind, Is.EqualTo(Kind.ToUnCheckable), "チェック不可に切り替えたので、 ToUnCheckable が入っている");
        }

        private CommentEditPageViewModel GetViewModel()
        {
            var param = new DialogParameters();

            var dbManager = new DatabaseManager(new DatabaseMock());
            param.Add(nameof(DatabaseManager), dbManager);

            var comment = new Comment();
            dbManager.Add(comment);
            param.Add(nameof(Comment), comment);

            var vm = new CommentEditPageViewModel();
            vm.OnDialogOpened(param);
            return vm;
        }
    }
}