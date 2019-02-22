namespace todo_cqrs.Core
{
    /// <summary>
    /// Singleton Pattern (Not thread-safe)
    /// </summary>
    public class Singleton
    {
        private static Singleton instance;
        private Singleton()
        {

        }

        public static Singleton Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Singleton();
                }
                return instance;
            }
        }
    }

    /// <summary>
    /// Pattern to create a singleton behavior for any reference type
    /// e.g. var totalScore = new GenericSingleton<Score>()
    /// </summary>
    /// <returns></returns>
    public class GenericSingleton<T> where T : new()
    {
        private static T instance;
        private GenericSingleton()
        {

        }

        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new T();
                }
                return instance;
            }
        }
    }
}