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
    }
}