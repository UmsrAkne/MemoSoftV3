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
            DataSource.Add(cm);
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
        }
    }
}