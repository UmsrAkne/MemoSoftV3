﻿using System.Collections.Generic;
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

        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        // DbContext の実装クラスに必須のため
        public DbSet<SubComment> SubComments { get; set; }

        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        // DbContext の実装クラスに必須のため
        public DbSet<Tag> Tags { get; set; }

        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        // DbContext の実装クラスに必須のため
        public DbSet<TagMap> TagMaps { get; set; }

        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        // DbContext の実装クラスに必須のため
        public DbSet<Group> Groups { get; set; }

        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        // DbContext の実装クラスに必須のため
        public DbSet<DatabaseAction> Actions { get; set; }

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
            SaveChanges();
        }

        public void Add(SubComment cm)
        {
            SubComments.Add(cm);
            SaveChanges();
        }

        public void Add(Tag tag)
        {
            Tags.Add(tag);
            SaveChanges();
        }

        public void Add(TagMap tagMap)
        {
            TagMaps.Add(tagMap);
            SaveChanges();
        }

        public void Add(Group group)
        {
            Groups.Add(group);
            SaveChanges();
        }

        public void Add(DatabaseAction action)
        {
            Actions.Add(action);
            SaveChanges();
        }

        public void EnsureCreated()
        {
            Database.EnsureCreated();
        }

        public void Save()
        {
            SaveChanges();
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