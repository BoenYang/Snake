
namespace FrameWork.Core
{
    public class ServiceModule<T> : Module where T : ServiceModule<T> ,new ()
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new T();
                }
                return _instance;
            }
        }
    }
}
