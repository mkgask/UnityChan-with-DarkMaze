using Storage;

namespace sgffu.Storage
{
    public class Service
    {
        public static void init()
        {
            PlayerPrefsAlt.init();
        }

        public static void save<T>(int slot, string key,  T obj)
        {
            PlayerPrefsAlt.Save($"Slot{slot}.{key}", obj);
        }

        public static T load<T>(int slot, string key)
        {
            return PlayerPrefsAlt.Load<T>($"Slot{slot}.{key}");
        }
    }
}