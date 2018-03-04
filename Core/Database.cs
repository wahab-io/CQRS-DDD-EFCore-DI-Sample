namespace todo_cqrs.Core
{
    public interface IDatabase
    {
        void AddEvent();
    }

    public class SQLiteDatabase : IDatabase
    {
        public SQLiteDatabase()
        {
            
        }
        public void AddEvent()
        {

        }
    }

    public class SQLServerDatabase : IDatabase
    {
        public void AddEvent()
        {

        }
    }
}