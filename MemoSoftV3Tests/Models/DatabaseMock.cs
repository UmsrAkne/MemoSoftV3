using MemoSoftV3.Models;

namespace MemoSoftV3Tests.Models
{
    public class DatabaseMock : IDataSource
    {
        private int commentId;
        private int subCommentId;
        private int tagId;
        private int tagMapId;
        private int groupId;
        private int actionId;

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
            if (cm.Id == 0)
            {
                cm.Id = ++commentId;
            }
            
            Comments.Add(cm);
        }

        public void Add(SubComment cm)
        {
            if (cm.Id == 0)
            {
                cm.Id = ++subCommentId;
            }
            
            SubComments.Add(cm);
        }

        public void Add(Tag tag)
        {
            if (tag.Id == 0)
            {
                tag.Id = ++tagId;
            }
            
            Tags.Add(tag);
        }

        public void Add(TagMap tagMap)
        {
            if (tagMap.Id == 0)
            {
                tagMap.Id = ++tagMapId;
            }
            
            TagMaps.Add(tagMap);
        }

        public void Add(Group group)
        {
            if (group.Id == 0)
            {
                group.Id = ++groupId;
            }
            
            Groups.Add(group);
        }

        public void Add(DatabaseAction action)
        {
            if (action.Id == 0)
            {
                action.Id = ++actionId;
            }
            
            Actions.Add(action);
        }
    }
}