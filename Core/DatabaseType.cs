using System;
using System.Collections.Generic;

namespace todo_cqrs.Core
{
    public class DatabaseType : Enumeration
    {
        public static DatabaseType InMemory = new DatabaseType(1, "InMemory");
        public static DatabaseType Sqlite = new DatabaseType(2, "Sqlite");
        public static DatabaseType PostgrSQL = new DatabaseType(3, "PostgrSQL");
        public static DatabaseType SqlServer = new DatabaseType(4, "SqlServer");
        public DatabaseType(int id, string name) : base(id, name)
        {

        }

        public static IEnumerable<DatabaseType> List()
        {
            return new[] { InMemory, Sqlite, PostgrSQL, SqlServer };
        }

        public static DatabaseType TryParse(string value)
        {
            if (string.Compare(value, DatabaseType.InMemory.ToString()) == 0)
                return DatabaseType.InMemory;
            if (string.Compare(value, DatabaseType.Sqlite.ToString()) == 0)
                return DatabaseType.Sqlite;
            if (string.Compare(value, DatabaseType.SqlServer.ToString()) == 0)
                return DatabaseType.SqlServer;
            if (string.Compare(value, DatabaseType.PostgrSQL.ToString()) == 0)
                return DatabaseType.PostgrSQL;
            throw new InvalidCastException();
        }
    }
}