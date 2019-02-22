namespace todo_cqrs.Core
{
    /// <summary>
    /// Thread Safe Singleton Pattern
    /// </summary>
    public class ThreadSafeSingleton
    {
        private static readonly object _lock = new object();
        private static ThreadSafeSingleton _instance;

        private ThreadSafeSingleton()
        {

        }

        public static ThreadSafeSingleton Instance 
        {
            get
            {
                if (_instance == null)
                {
                    lock(_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new ThreadSafeSingleton();
                        }
                    }
                }
                return _instance;
            }
        }
    }
}