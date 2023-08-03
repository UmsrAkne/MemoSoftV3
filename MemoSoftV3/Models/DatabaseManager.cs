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
        }

        private IDataSource DataSource { get; set; }

        public void Add(Comment cm)
        {
            var belongToGroup = cm.GroupId != 0;
            var existsReferenceGroup = DataSource.GetGroups().Any(g => g.Id == cm.GroupId);
            if (belongToGroup && !existsReferenceGroup)
            {
                // コメントがいずれかのグループに所属しているのに、参照先のグループが存在しない場合は処理を中断する。
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

        public List<Comment> SearchComments(SearchOption option)
        {
            if (option.IsDefault)
            {
                return DataSource.GetComments().ToList();
            }

            // 上から順番に、テキスト - グループ - 追加日時 の順番でフィルタリング。
            var comments = DataSource.GetComments()
                .Where(c => c.Text.Contains(option.Text) || string.IsNullOrEmpty(option.Text))
                .Join(
                    DataSource.GetGroups(),
                    c => c.GroupId,
                    g => g.Id,
                    (c, g) =>
                    {
                        c.GroupName = g.Name;
                        return c;
                    })
                .Where(c => c.GroupName.Contains(option.GroupName) || string.IsNullOrEmpty(option.GroupName))
                .Join(
                    DataSource.GetActions().Where(a => a.Target == Target.Comment && a.Kind == Kind.Add),
                    c => c.Id,
                    a => a.TargetId,
                    (c, a) =>
                    {
                        c.DateTime = a.DateTime;
                        return c;
                    })
                .Where(c => option.StartDateTime < c.DateTime && option.EndDateTime > c.DateTime)
                .ToList();

            if (!option.TagTexts.Any())
            {
                // タグのフィルタリングは、二度 Join が必要になるので、タグが未入力の場合は終了。
                return comments;
            }

            var matchedTagMaps = DataSource.GetTags()
                .Where(t => option.TagTexts.Contains(t.Name))
                .Join(
                    DataSource.GetTagMaps(),
                    t => t.Id,
                    tm => tm.TagId,
                    (_, tm) => tm)
                .ToList();

            return comments.Join(
                    matchedTagMaps,
                    c => c.Id,
                    tm => tm.CommentId,
                    (c, _) => c)
                .ToList();
        }

        public List<Comment> InjectCommentProperties(IEnumerable<Comment> comments)
        {
            return comments
                .Join(
                    DataSource.GetActions().Where(a => a.Kind == Kind.Add && a.Target == Target.Comment),
                    c => c.Id,
                    a => a.TargetId,
                    (c, a) =>
                    {
                        c.DateTime = a.DateTime;
                        return c;
                    })
                .Join(
                    DataSource.GetGroups(),
                    c => c.GroupId,
                    g => g.Id,
                    (c, g) =>
                    {
                        c.GroupName = g.Name;
                        return c;
                    })
                .GroupJoin(
                    DataSource.GetSubComments(),
                    c => c.Id,
                    sc => sc.ParentCommentId,
                    (c, scs) =>
                    {
                        c.SubComments = scs.Join(
                            DataSource.GetActions().Where(a => a.Kind == Kind.Add && a.Target == Target.SubComment),
                            s => s.Id,
                            a => a.TargetId,
                            (s, a) =>
                            {
                                s.DateTime = a.DateTime;
                                return s;
                            }).ToList();

                        if (c.SubComments.Count(sc => sc.TimeTracking) >= 2)
                        {
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
                        }

                        return c;
                    })
                .GroupJoin(
                    DataSource.GetTagMaps(),
                    c => c.Id,
                    t => t.CommentId,
                    (c, ts) =>
                    {
                        c.Tags = ts.Join(
                            DataSource.GetTags(),
                            tm => tm.TagId,
                            t => t.Id,
                            (_, t) => t).ToList();

                        return c;
                    }).ToList();
        }
    }
}