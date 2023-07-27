using MemoSoftV3.Models;

namespace MemoSoftV3Tests.Models
{
    public class DatabaseMock : IDataSource
    {
        public List<Comment> Comments { get; } = new ();

        public List<Tag> Tags { get; } = new ();

        private List<TagMap> TagMaps { get; } = new ();

        public IEnumerable<Comment> GetComments()
        {
            return Comments;
        }

        public IEnumerable<Tag> GetTags()
        {
            return Tags;
        }

        public IEnumerable<TagMap> GetTagMaps()
        {
            return TagMaps;
        }

        public void Add(Comment cm)
        {
            Comments.Add(cm);
        }

        public void Add(Tag tag)
        {
            Tags.Add(tag);
        }

        public void Add(TagMap tagMap)
        {
            TagMaps.Add(tagMap);
        }
    }
}