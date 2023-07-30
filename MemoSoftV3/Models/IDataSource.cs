using System.Collections.Generic;

namespace MemoSoftV3.Models
{
    public interface IDataSource
    {
        public IEnumerable<Comment> GetComments();

        public IEnumerable<SubComment> GetSubComments();

        public IEnumerable<Tag> GetTags();

        public IEnumerable<TagMap> GetTagMaps();

        public IEnumerable<Group> GetGroups();

        public IEnumerable<DatabaseAction> GetActions();

        public void Add(Comment cm);

        public void Add(SubComment cm);

        public void Add(Tag tag);

        public void Add(TagMap tagMap);

        public void Add(Group group);

        public void Add(DatabaseAction action);
    }
}