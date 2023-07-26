using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace MemoSoftV3.Models
{
    public class DatabaseContext : DbContext, IDataSource
    {
        private const string DatabaseFileName = "commentDb.sqlite";

        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        // DbContext の実装クラスに必須のため
        public DbSet<Comment> Comments { get; set; }

        public IEnumerable<Comment> GetComments()
        {
            return Comments;
        }

        public void Add(Comment cm)
        {
            Comments.Add(cm);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!File.Exists(DatabaseFileName))
            {
                SQLiteConnection.CreateFile(DatabaseFileName); // ファイルが存在している場合は問答無用で上書き。
            }

            var connectionString = new SqliteConnectionStringBuilder { DataSource = DatabaseFileName, }.ToString();
            optionsBuilder.UseSqlite(new SQLiteConnection(connectionString));
        }
    }
}