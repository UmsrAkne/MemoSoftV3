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

        [Test]
        public void AddTagMapTest()
        {
            var source = new DatabaseMock();
            source.Tags.Add(new Tag() { Id = 1, Name = "a", });
            source.Comments.Add(new Comment() { Id = 1, Text = "test", });

            var tagMap = new TagMap()
            {
                Id = 1, TagId = 1, CommentId = 1,
            };

            var manager = new DatabaseManager(source);
            manager.Add(tagMap);

            Assert.Multiple(() =>
            {
                Assert.That(source.GetTagMaps().First(), Is.EqualTo(tagMap));
                Assert.That(source.GetTagMaps().Count(), Is.EqualTo(1));
            });
        }

        [Test]
        [TestCase(1, -1, 1)]
        [TestCase(1, 1, -1)]
        [TestCase(1, -1, -1)]
        [TestCase(1, 2, 1)]
        [TestCase(1, 1, 2)]
        [TestCase(1, 2, 2)]
        public void AddTagMapTest_無効な入力(int id, int tagId, int commentId)
        {
            var source = new DatabaseMock();
            source.Tags.Add(new Tag() { Id = 1, Name = "a", });
            source.Comments.Add(new Comment() { Id = 1, Text = "test", });

            var tagMap = new TagMap()
            {
                Id = id, TagId = tagId, CommentId = commentId,
            };

            var manager = new DatabaseManager(source);
            manager.Add(tagMap);

            Assert.Multiple(() =>
            {
                Assert.That(source.GetTagMaps().Count(), Is.Zero, "無効な値を入力したので中身は空");
            });
        }

        [Test]
        public void AddTagMapTest_重複入力()
        {
            var source = new DatabaseMock();
            source.Tags.Add(new Tag() { Id = 1, Name = "a", });
            source.Comments.Add(new Comment() { Id = 1, Text = "test", });

            var tagMap = new TagMap() { Id = 1, TagId = 1, CommentId = 1, };
            var tagMap2 = new TagMap() { Id = 2, TagId = 1, CommentId = 1, };

            var manager = new DatabaseManager(source);
            manager.Add(tagMap);
            manager.Add(tagMap2);

            Assert.Multiple(() =>
            {
                Assert.That(source.GetTagMaps().Count(), Is.EqualTo(1), "重複として判定されるはずの要素を２つ入れたので片方弾かれて 1");
            });
        }

        [Test]
        public void AddGroupTest_正常系()
        {
            var source = new DatabaseMock();
            var group = new Group { Id = 1, Name = "testGroup", };
            var manager = new DatabaseManager(source);
            manager.Add(group);

            Assert.Multiple(() =>
            {
                Assert.That(source.GetGroups().Count(), Is.EqualTo(1));
                Assert.That(source.GetGroups().FirstOrDefault(), Is.EqualTo(group));
            });
        }

        [Test]
        public void AddGroupTest_名前が空文字のグループ()
        {
            var source = new DatabaseMock();
            var group = new Group { Id = 1, Name = string.Empty, };
            var manager = new DatabaseManager(source);
            manager.Add(group);

            Assert.Multiple(() =>
            {
                Assert.That(source.GetGroups().Count(), Is.EqualTo(0));
                Assert.That(source.GetGroups().FirstOrDefault(), Is.Null);
            });
        }

        [Test]
        public void AddComment_異常なグループを持ったコメントの追加()
        {
            var source = new DatabaseMock();
            var comment = new Comment { GroupId = 2, }; // GroupId == 2 は存在しない。
            var manager = new DatabaseManager(source);
            manager.Add(comment);

            Assert.That(source.GetComments().Count(), Is.EqualTo(0), "コメントは不正な値のはずなので、追加処理はされていないはず。");
        }
    }
}