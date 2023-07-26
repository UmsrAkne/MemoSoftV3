using System.Collections.Generic;

namespace MemoSoftV3.Models
{
    public interface IDataSource
    {
        public IEnumerable<Comment> GetComments();

        public void Add(Comment cm);
    }
}