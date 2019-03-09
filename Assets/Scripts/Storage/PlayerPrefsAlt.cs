using System;
using System.Collections.Generic;
using UnityEngine;
using MessagePack;

using Dir = Filesystem.Dir;
using File = Filesystem.File;

namespace Storage {

#if UNITY_STANDALONE

    class PlayerPrefsAlt
    {
        private static string _filename = "/storage/savedata.json";

        private static File _save_file = new File();

        private static Dictionary<string, string> _save_data = new Dictionary<string, string>();

        public static void init()
        {
            Dir.create(Application.dataPath + _filename);
            _save_file.reset(_filename, "");

            var str = _save_file.read();
            Debug.Log("PlayerPrefsAlt.init() : str.Length : " + str.Length.ToString());
            Debug.Log("PlayerPrefsAlt.init() : str : " + str);

            if (0 < str.Length) {
                //_save_data = JsonUtility.FromJson<Dictionary<string, string>>(str);
                _save_data = MessagePackSerializer.Deserialize<Dictionary<string, string>>(
                    MessagePackSerializer.FromJson(str)
                );
            }
        }

        public static void Save<T>(string key, T obj)
        {
            var bytes = MessagePackSerializer.Serialize(obj);
            var str = Convert.ToBase64String(bytes);
            //PlayerPrefs.SetString(key, str);
            SetString(key, str);
        }

        public static T Load<T>(string key)
        {
            //var str = PlayerPrefs.GetString(key);
            var str = GetString(key);
            Debug.Log("PlayerPrefsAlt.Load() : str.Length : " + str.Length.ToString());
            if (str.Length < 1) return default(T);
            var bytes = Convert.FromBase64String(str);
            Debug.Log("PlayerPrefsAlt.Load() : bytes.Length : " + bytes.Length.ToString());
            return MessagePackSerializer.Deserialize<T>(bytes);
        }

        public static void Delete(string key)
        {
            PlayerPrefs.DeleteKey(key);
        }

        public static void SetString(string key, string str)
        {
            if (_save_data.ContainsKey(key)) {
                _save_data[key] = str;
            } else {
                _save_data.Add(key, str);
            }

            _save_file.write(MessagePackSerializer.ToJson(_save_data), "", Filesystem.File.append_type.None);
            return;
        }

        public static string GetString(string key)
        {
            if (_save_data.ContainsKey(key)) return _save_data[key];
            return "";
        }
    }

#else

    class PlayerPrefsAlt
    {

        public static void init()
        {
        }

        public static void Save<T>(string key, T obj)
        {
            var bytes = MessagePackSerializer.Serialize(obj);
            var str = Convert.ToBase64String(bytes);
            PlayerPrefs.SetString(key, str);
        }

        public static T Load<T>(string key)
        {
            var str = PlayerPrefs.GetString(key);
            if (str.Length < 1) return default(T);
            var bytes = Convert.FromBase64String(str);
            return MessagePackSerializer.Deserialize<T>(bytes);
        }

        public static void Delete(string key)
        {
            PlayerPrefs.DeleteKey(key);
        }
    }

#endif

}