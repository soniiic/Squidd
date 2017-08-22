using System;
using System.Data.Common;
using System.Data.SqlClient;

namespace Squidd.Commander.DAL
{
    public abstract class SqlBaseRepository
    {
        private readonly string connectionString;

        public SqlBaseRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        internal TReturn Run<TReturn>(Func<DbConnection, TReturn> action)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                return action(conn);
            }
        }

        internal void Run(Action<DbConnection> action)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                action(conn);
            }
        }
    }
}
