using MemoSoftV3.Models;

namespace MemoSoftV3Tests.Models
{
    public class DatabaseMock : IDataSource
    {
        public List<Comment> Comments { get; } = new ();

        public List<Tag> Tags { get; } = new ();

        public List<SubComment> SubComments { get; } = new ();

        private List<TagMap> TagMaps { get; } = new ();

        private List<Group> Groups { get; } = new ();

        private List<DatabaseAction> Actions { get; } = new ();

        public IEnumerable<Comment> GetComments()
        {
            return Comments;
        }

        public IEnumerable<SubComment> GetSubComments()
        {
            return SubComments;
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

        public IEnumerable<DatabaseAction> GetActions()
        {
            return Actions;
        }

        public void Add(Comment cm)
        {
            Comments.Add(cm);
        }

        public void Add(SubComment cm)
        {
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

        public void Add(DatabaseAction action)
        {
            Actions.Add(action);
        }
    }
}