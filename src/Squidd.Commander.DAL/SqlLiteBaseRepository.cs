using System;
using System.Data.Common;
using System.Data.SQLite;
using System.IO;
using Dapper;

namespace Squidd.Commander.DAL
{
    public abstract class SqlLiteBaseRepository
    {
        private readonly string baseDirectory;

        protected SqlLiteBaseRepository(string baseDirectory)
        {
            this.baseDirectory = baseDirectory;
            if (!File.Exists(DbFile))
            {
                CreateDatabase();
            }
        }

        private void CreateDatabase()
        {
            using (var cnn = SimpleDbConnection())
            {
                cnn.Open();
                cnn.Execute(
                    @"CREATE TABLE `Runner` (
	`Id`	INTEGER PRIMARY KEY AUTOINCREMENT,
	`EndPoint`	TEXT,
	`ComputerName`	TEXT,
	`FriendlyName`	TEXT
);");
            }
        }

        private string DbFile => baseDirectory + "\\SimpleDb.sqlite";

        private SQLiteConnection SimpleDbConnection()
        {
            return new SQLiteConnection("Data Source=" + DbFile);
        }

        internal TReturn Run<TReturn>(Func<DbConnection, TReturn> func)
        {
            using (var conn = SimpleDbConnection())
            {
                conn.Open();
                return func(conn);
            }
        }

        internal void Run(Action<DbConnection> action)
        {
            using (var conn = SimpleDbConnection())
            {
                conn.Open();
                action(conn);
            }
        }
    }
}