using MemoSoftV3.Models;
using SearchOption = MemoSoftV3.Models.SearchOption;

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

        [Test]
        public void AddComment_スマートグループに所属するコメントの追加()
        {
            var source = new DatabaseMock();
            var comment = new Comment { GroupId = 2, }; // GroupId == 2 はスマートグループ

            var manager = new DatabaseManager(source);
            manager.Add(new Group { IsSmartGroup = true, Id = 2, Name = "TestGroup", });
            manager.Add(comment);

            Assert.That(source.GetComments().Count(), Is.EqualTo(0), "コメントは不正な値のはずなので、追加処理はされていないはず。");
        }

        [Test]
        public void AddSubComment_存在しない親を指定()
        {
            var source = new DatabaseMock();
            var comment = new SubComment { ParentCommentId = 1, }; // コメントは一つも入ってないので、親コメントは存在しない。
            var manager = new DatabaseManager(source);
            manager.Add(comment);

            Assert.That(source.GetSubComments().Count(), Is.EqualTo(0), "存在しないコメントを親にしたので、追加処理はされてないはず");
        }

        [Test]
        public void DatabaseAction追加のテスト()
        {
            var source = new DatabaseMock();

            var group = new Group { Id = 1, Name = "testGroup", };
            var comment = new Comment { Id = 2, GroupId = 1, Text = "testComment", };
            var tag = new Tag { Id = 3, Name = "testTag", };
            var tagMap = new TagMap { Id = 4, TagId = 3, CommentId = 2, };
            var subComment = new SubComment { Id = 5, ParentCommentId = 2, Text = "testSubComment", };

            var manager = new DatabaseManager(source);
            manager.Add(group);
            manager.Add(comment);
            manager.Add(tag);
            manager.Add(tagMap);
            manager.Add(subComment);

            var acs = source.GetActions().ToList();

            var g = acs.First(a => a is { Kind: Kind.Add, Target: Target.Group, });
            var c = acs.First(a => a is { Kind: Kind.Add, Target: Target.Comment, });
            var t = acs.First(a => a is { Kind: Kind.Add, Target: Target.Tag, });
            var tm = acs.First(a => a is { Kind: Kind.Add, Target: Target.TagMap, });
            var sc = acs.First(a => a is { Kind: Kind.Add, Target: Target.SubComment, });

            // TargetId が間違いなくセットされているか確認する。
            Assert.Multiple(() =>
            {
                Assert.That(g, !Is.Null);
                Assert.That(g.TargetId, Is.EqualTo(1));
                Assert.That(c, !Is.Null);
                Assert.That(c.TargetId, Is.EqualTo(2));
                Assert.That(t, !Is.Null);
                Assert.That(t.TargetId, Is.EqualTo(3));
                Assert.That(tm, !Is.Null);
                Assert.That(tm.TargetId, Is.EqualTo(4));
                Assert.That(sc, !Is.Null);
                Assert.That(sc.TargetId, Is.EqualTo(5));
            });
        }

        [Test]
        public void GetCommentsTest()
        {
            var source = new DatabaseMock();

            var manager = new DatabaseManager(source);
            manager.Add(new Group { Id = 1, Name = "defaultGroup", });
            manager.Add(new Group { Id = 2, Name = "otherGroup", });
            manager.Add(new Comment { Id = 1, GroupId = 1, Text = "testComment", });
            manager.Add(new Comment { Id = 2, GroupId = 1, Text = "nextComment", });
            manager.Add(new Comment { Id = 3, GroupId = 1, Text = "thirdComment", });
            manager.Add(new Comment { Id = 4, GroupId = 2, Text = "test", });

            manager.Add(new Tag { Id = 1, Name = "testTag", });
            manager.Add(new TagMap { Id = 1, TagId = 1, CommentId = 2, });
            manager.Add(new TagMap { Id = 2, TagId = 1, CommentId = 3, });

            var comments = manager.SearchComments(new SearchOption
            {
                Text = "Comment",
                StartDateTime = default,
                EndDateTime = DateTime.MaxValue,
                TagTexts = new List<string> { "testTag", },
                GroupName = "defaultGroup",
            });

            Assert.That(comments, Has.Count.EqualTo(2));
        }

        [Test]
        public void InjectPropertiesTest()
        {
            var source = new DatabaseMock();

            var manager = new DatabaseManager(source);
            manager.Add(new Group { Id = 1, Name = "defaultGroup", });
            manager.Add(new Group { Id = 2, Name = "otherGroup", });
            manager.Add(new Comment { Id = 1, GroupId = 1, Text = "testComment", });
            manager.Add(new Comment { Id = 2, GroupId = 2, Text = "nextComment", });
            manager.Add(new Tag { Id = 1, Name = "testTagA", });
            manager.Add(new Tag { Id = 2, Name = "testTagB", });
            manager.Add(new Tag { Id = 3, Name = "testTagC", });
            manager.Add(new TagMap { Id = 1, TagId = 1, CommentId = 1, });
            manager.Add(new TagMap { Id = 2, TagId = 2, CommentId = 1, });
            manager.Add(new TagMap { Id = 3, TagId = 3, CommentId = 1, });
            manager.Add(new TagMap { Id = 4, TagId = 1, CommentId = 2, });
            manager.Add(new TagMap { Id = 5, TagId = 2, CommentId = 2, });
            manager.Add(new SubComment() { Id = 1, Text = "subCommentA", ParentCommentId = 1, TimeTracking = true, });
            manager.Add(new SubComment() { Id = 2, Text = "subCommentB", ParentCommentId = 1, TimeTracking = true, });
            manager.Add(new SubComment() { Id = 3, Text = "subCommentC", ParentCommentId = 2, });

            source.GetActions()
                .Where(a => a.Kind == Kind.Add && a.Target == Target.SubComment)
                .ToList()[0].DateTime = new DateTime(2023, 8, 4, 23, 0, 0);

            source.GetActions()
                .Where(a => a.Kind == Kind.Add && a.Target == Target.SubComment)
                .ToList()[1].DateTime = new DateTime(2023, 8, 4, 23, 10, 0);

            var comments = manager.SearchComments(new SearchOption());
            comments = manager.InjectCommentProperties(comments);
            var comment1 = comments[0];
            var comment2 = comments[1];

            Assert.That(comment1.GroupName, Is.EqualTo("defaultGroup"));
            Assert.That(comment1.Tags, Has.Count.EqualTo(3));
            Assert.That(comment1.SubComments, Has.Count.EqualTo(2));

            Assert.That(comment2.GroupName, Is.EqualTo("otherGroup"));
            Assert.That(comment2.Tags, Has.Count.EqualTo(2));
            Assert.That(comment2.SubComments, Has.Count.EqualTo(1));

            Assert.That(comment1.SubComments[1].WorkingTimeSpan, Is.EqualTo(TimeSpan.FromMinutes(10)),
                "設定した時刻から、WorkingTimeSpan は10分間となるはず");
        }

        [Test]
        public void GetCommentsTest_デフォルト検索オプション()
        {
            var manager = GetSampleCommentList();
            var comments = manager.GetComments(new SearchOption());
            Assert.That(comments, Has.Count.EqualTo(3));

            Assert.That(comments[0].Tags, Has.Count.EqualTo(2));
            Assert.That(comments[0].GroupName, Is.EqualTo("defaultGroup"));
            Assert.That(comments[0].SubComments, Has.Count.EqualTo(2));
            Assert.That(comments[0].SubComments[0].Text, Is.EqualTo("subCommentA"));
            Assert.That(comments[0].SubComments[1].Text, Is.EqualTo("subCommentB"));

            Assert.That(comments[1].Tags, Has.Count.EqualTo(1));
            Assert.That(comments[1].GroupName, Is.EqualTo("otherGroup"));
            Assert.That(comments[1].SubComments, Has.Count.EqualTo(1));
            Assert.That(comments[1].SubComments[0].Text, Is.EqualTo("subCommentC"));

            Assert.That(comments[2].GroupName, Is.EqualTo("otherGroup"));
            Assert.That(comments[2].Tags, Has.Count.EqualTo(0));
        }

        [Test]
        public void GetCommentsTest_テキスト検索オプション()
        {
            var manager = GetSampleCommentList();
            var searchOption = new SearchOption { Text = "next", };
            var comments = manager.GetComments(searchOption);

            Assert.That(comments, Has.Count.EqualTo(2));

            Assert.That(comments[0].Text, Is.EqualTo("nextComment"));
            Assert.That(comments[1].Text, Is.EqualTo("nextComment3"));
        }

        [Test]
        public void GetCommentsTest_テキスト_タグ検索()
        {
            var manager = GetSampleCommentList();
            var searchOption = new SearchOption { Text = "next", TagTexts = { "testTagA", }, };
            var comments = manager.GetComments(searchOption);

            Assert.That(comments, Has.Count.EqualTo(1));
            Assert.That(comments[0].Text, Is.EqualTo("nextComment"));
        }

        [Test]
        [TestCase("defaultGroup", 1)]
        [TestCase("otherGroup", 2)]
        public void GetCommentsTest_グループ検索(string groupName, int groupCount)
        {
            var manager = GetSampleCommentList();
            var searchOption = new SearchOption { GroupName = groupName, };
            var comments = manager.GetComments(searchOption);

            Assert.That(comments, Has.Count.EqualTo(groupCount));
        }

        private DatabaseManager GetSampleCommentList()
        {
            var source = new DatabaseMock();

            var manager = new DatabaseManager(source);
            manager.Add(new Group { Name = "defaultGroup", });
            manager.Add(new Group { Name = "otherGroup", });
            manager.Add(new Comment { GroupId = 1, Text = "testComment", });
            manager.Add(new Comment { GroupId = 2, Text = "nextComment", });
            manager.Add(new Comment { GroupId = 2, Text = "nextComment3", });
            manager.Add(new Tag { Name = "testTagA", });
            manager.Add(new Tag { Name = "testTagB", });
            manager.Add(new Tag { Name = "testTagC", });
            manager.Add(new TagMap { TagId = 1, CommentId = 1, });
            manager.Add(new TagMap { TagId = 2, CommentId = 1, });
            manager.Add(new TagMap { TagId = 1, CommentId = 2, });
            manager.Add(new SubComment { Text = "subCommentA", ParentCommentId = 1, TimeTracking = true, });
            manager.Add(new SubComment { Text = "subCommentB", ParentCommentId = 1, TimeTracking = true, });
            manager.Add(new SubComment { Text = "subCommentC", ParentCommentId = 2, });

            return manager;
        }
    }
}