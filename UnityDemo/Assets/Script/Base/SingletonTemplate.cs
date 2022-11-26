
/// <summary>
/// 非线程安全单件模板
/// </summary>
/// <typeparam name="T"></typeparam>

namespace Base
{
    public class SingletonTemplate<T> where T : class, new()
    {
        protected static T singleton = null;

        public static T Singleton
        {
            get
            {
                if (singleton == null)
                {
                    singleton = new T();
                }
                return singleton;
            }
        }

        public static void DestorySelf()
        {
            singleton = null;
        }

    }
}

