using MemoSoftV3.Models;

namespace MemoSoftV3Tests.Models
{
    [TestFixture]
    public class DatabaseManagerTest
    {
        [Test]
        public void AddCommentTest()
        {
            var source = new DatabaseMock();
            var manager = new DatabaseManager(source);
            var comment = new Comment();
            manager.Add(comment);

            Assert.That(source.GetComments().First(), Is.EqualTo(comment));
        }

        [Test]
        public void AddTagTest_正常系()
        {
            var source = new DatabaseMock();
            var manager = new DatabaseManager(source);
            var tag = new Tag() { Id = 1, Name = "a", };
            manager.Add(tag);

            Assert.That(source.GetTags().First(), Is.EqualTo(tag));
        }

        [Test]
        public void AddTagTest_空文字入力()
        {
            var source = new DatabaseMock();
            var manager = new DatabaseManager(source);
            var tag = new Tag() { Id = 1, Name = string.Empty, };
            manager.Add(tag);

            Assert.IsNull(source.GetTags().FirstOrDefault());
        }

        [Test]
        public void AddTagTest_重複入力()
        {
            var source = new DatabaseMock();
            var manager = new DatabaseManager(source);
            var tag1 = new Tag() { Id = 1, Name = "a", };
            var tag2 = new Tag() { Id = 2, Name = "a", };
            manager.Add(tag1);
            manager.Add(tag2);

            Assert.That(source.GetTags().First(), Is.EqualTo(tag1), "tag1 が先に追加されるので、tag2 は入力されない。");
            Assert.That(source.GetTags().Count(), Is.EqualTo(1), "入力は２回だが、実際に入力されるのは１回目のみ");
        }
    }
}