using System;
using System.Collections.Generic;
using System.Linq;

namespace MemoSoftV3.Models
{
    public class DatabaseManager
    {
        public DatabaseManager(IDataSource source)
        {
            DataSource = source;
            DataSource.EnsureCreated();
        }

        private IDataSource DataSource { get; set; }

        public void Add(Comment cm)
        {
            var belongToGroup = cm.GroupId != 0;
            var targetGroup = DataSource.GetGroups().FirstOrDefault(g => g.Id == cm.GroupId);

            if (belongToGroup && (targetGroup == null || targetGroup.IsSmartGroup))
            {
                // コメントがいずれかのグループに所属しているのに、参照先のグループが存在しないか、スマートグループである場合は処理を中断する。
                return;
            }

            DataSource.Add(cm);
            DataSource.Add(new DatabaseAction(cm, Kind.Add));
        }

        public void Add(SubComment cm)
        {
            var existsParentComment = DataSource.GetComments().Any(c => c.Id == cm.ParentCommentId);
            if (!existsParentComment)
            {
                // 親コメントが存在しない場合は終了する。
                return;
            }

            DataSource.Add(cm);
            DataSource.Add(new DatabaseAction(cm, Kind.Add));
        }

        /// <summary>
        ///     テーブルにタグを追加します。
        ///     tag.Name が重複するタグが入力された場合、tag.Name が空文字か null だった場合は、追加せずにメソッドを終了します。
        /// </summary>
        /// <param name="tag">追加するタグです。</param>
        public void Add(Tag tag)
        {
            var isEmpty = string.IsNullOrEmpty(tag.Name);
            var containsSame = DataSource.GetTags().Any(t => t.Name == tag.Name);

            if (isEmpty || containsSame)
            {
                return;
            }

            DataSource.Add(tag);
            DataSource.Add(new DatabaseAction(tag, Kind.Add));
        }

        public void Add(TagMap tagMap)
        {
            // 0以下の Id は不正な Id
            var isIllegalId = tagMap.TagId <= 0 || tagMap.CommentId <= 0;

            // リンク先のタグとコメントが存在するか
            var existsTag = DataSource.GetTags().Any(t => t.Id == tagMap.TagId);
            var existsComment = DataSource.GetComments().Any(c => c.Id == tagMap.CommentId);

            // 重複チェック
            var containsSame = DataSource.GetTagMaps()
                .Any(t => t.TagId == tagMap.TagId && t.CommentId == tagMap.CommentId);

            if (isIllegalId || !existsTag || !existsComment || containsSame)
            {
                return;
            }

            DataSource.Add(tagMap);
            DataSource.Add(new DatabaseAction(tagMap, Kind.Add));
        }

        public void Add(Group group)
        {
            if (string.IsNullOrWhiteSpace(group.Name))
            {
                return;
            }

            DataSource.Add(group);
            DataSource.Add(new DatabaseAction(group, Kind.Add));
        }

        public void Add(DatabaseAction action)
        {
            DataSource.Add(action);
        }

        public void ExecuteCli(string text, CliOption cliOption)
        {
            var comment = new Comment
            {
                IsFavorite = cliOption.IsFavorite,
                IsCheckable = cliOption.Checkable,
                Text = text,
            };

            Add(comment);

            if (!string.IsNullOrEmpty(cliOption.GroupName))
            {
                // cliOption で指定されているグループをコメントに入力。存在しない場合は新規作成
                var group = DataSource.GetGroups().FirstOrDefault(g => g.Name == cliOption.GroupName);
                if (group == null)
                {
                    group = new Group { Name = cliOption.GroupName, };
                    DataSource.Add(group);
                }

                comment.GroupId = group.Id;
            }

            if (cliOption.Tags.Any())
            {
                // cliOption の指定タグを入力。存在しない場合は新規追加
                comment.Tags = cliOption.Tags.Select(ts =>
                {
                    var tag = DataSource.GetTags().SingleOrDefault(t => t.Name == ts);
                    return tag ?? new Tag { Name = ts, };
                }).ToList();

                foreach (var commentTag in comment.Tags)
                {
                    Add(commentTag);
                    Add(new TagMap { TagId = commentTag.Id, CommentId = comment.Id, });
                }
            }
        }

        public List<Comment> GetComments(SearchOption option)
        {
            var comments = DataSource.GetComments()
                .Where(c => c.Text.Contains(option.Text) || string.IsNullOrEmpty(option.Text)) // テキストでフィルタ
                .Join(
                    DataSource.GetGroups().Concat(new Group[] { new () { Id = 0, Name = string.Empty, }, }),
                    c => c.GroupId,
                    g => g.Id,
                    (c, g) =>
                    {
                        // 取り出したリストに Group のリストを Concat しているのは、GroupId == 0 のコメントも一緒に抽出するため。
                        c.GroupName = g.Name;
                        return c;
                    })
                .Where(c => c.GroupName.Contains(option.GroupName)
                            || string.IsNullOrEmpty(option.GroupName)) // グループ名でフィルタ
                .Join(
                    DataSource.GetActions().Where(a => a.Target == Target.Comment && a.Kind == Kind.Add),
                    c => c.Id,
                    a => a.TargetId,
                    (c, a) =>
                    {
                        // コメントのプロパティに日時を入力する。
                        c.DateTime = a.DateTime;
                        return c;
                    })
                .Where(c => option.StartDateTime < c.DateTime && option.EndDateTime > c.DateTime) // 日時でフィルタ
                .GroupJoin(
                    DataSource.GetTagMaps(),
                    c => c.Id,
                    t => t.CommentId,
                    (c, ts) =>
                    {
                        // コメントに紐づけされたタグを入力する。
                        c.Tags = ts.Join(
                            DataSource.GetTags(),
                            tm => tm.TagId,
                            t => t.Id,
                            (_, t) => t).ToList();

                        return c;
                    })
                .GroupJoin(
                    DataSource.GetSubComments(),
                    c => c.Id,
                    sc => sc.ParentCommentId,
                    (c, scs) =>
                    {
                        // コメントにサブコメントを入力する。
                        c.SubComments = scs.Join(
                            DataSource.GetActions().Where(a => a.Kind == Kind.Add && a.Target == Target.SubComment),
                            s => s.Id,
                            a => a.TargetId,
                            (s, a) =>
                            {
                                s.DateTime = a.DateTime;
                                return s;
                            }).ToList();

                        // サブコメントにタイムトラッキングが設定されている場合は、これに関する情報を入力する。
                        if (c.SubComments.Count(sc => sc.TimeTracking) < 2)
                        {
                            return c;
                        }

                        var dt = DateTime.MinValue;
                        foreach (var subComment in c.SubComments.Where(sc => sc.TimeTracking))
                        {
                            if (dt == DateTime.MinValue)
                            {
                                dt = subComment.DateTime;
                                continue;
                            }

                            subComment.WorkingTimeSpan = subComment.DateTime - dt;
                            dt = subComment.DateTime;
                        }

                        return c;
                    });

            if (!option.TagTexts.Any())
            {
                // 検索オプションのタグの情報がない場合は、この時点で終了する。
                return comments.ToList();
            }

            // タグの検索情報がある場合は、最後にタグでフィルタリングする。
            return comments
                .Where(c => c.Tags.Any(t => option.TagTexts.Any(tt => tt.Contains(t.Name)))) // タグでフィルタ
                .ToList();
        }

        public void ReloadSubCommentTimeTracking(IEnumerable<SubComment> subComments)
        {
            var timeTrackingComments = subComments.Where(sc => sc.TimeTracking).ToList();

            if (timeTrackingComments.Count < 2)
            {
                return;
            }

            var dt = DateTime.MinValue;
            foreach (var subComment in timeTrackingComments)
            {
                if (dt == DateTime.MinValue)
                {
                    dt = subComment.DateTime;
                    continue;
                }

                subComment.WorkingTimeSpan = subComment.DateTime - dt;
                dt = subComment.DateTime;
            }
        }

        public List<Group> GetGroups(SearchOption searchOption)
        {
            return string.IsNullOrEmpty(searchOption.GroupName)
                ? DataSource.GetGroups().ToList()
                : DataSource.GetGroups().Where(g => g.Name.Contains(searchOption.GroupName)).ToList();
        }

        public List<Tag> GetTags(SearchOption searchOption)
        {
            if (searchOption.TagTexts.Count == 0)
            {
                return DataSource.GetTags().ToList();
            }

            return DataSource.GetTags()
                .Where(t => searchOption.TagTexts
                    .Any(x => x.Contains(t.Name)))
                .ToList();
        }
    }
}