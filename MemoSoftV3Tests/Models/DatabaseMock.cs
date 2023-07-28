using MemoSoftV3.Models;

namespace MemoSoftV3Tests.Models
{
    public class DatabaseMock : IDataSource
    {
        public List<Comment> Comments { get; } = new ();

        public List<Tag> Tags { get; } = new ();

        private List<TagMap> TagMaps { get; } = new ();

        private List<Group> Groups { get; } = new ();

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

        public IEnumerable<Group> GetGroups()
        {
            return Groups;
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

        public void Add(Group group)
        {
            Groups.Add(group);
        }
    }
}