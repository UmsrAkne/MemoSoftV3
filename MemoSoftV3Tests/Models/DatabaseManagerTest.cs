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
    }
}