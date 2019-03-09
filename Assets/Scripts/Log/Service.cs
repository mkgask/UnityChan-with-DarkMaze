using System;
using System.IO;
using UnityEngine;
using UniCommon;
using Filesystem;


namespace sgffu.Log
{
    public class Service
    {
        public static string path = "";

        public static IStreamLogger<FileStream> logger;

        public static void init()
        {
            path = Application.persistentDataPath + "/Storage/logs/logs-" + System.DateTime.Now.ToString("yyyyMMdd-HHmmss") + ".log";
            var dir = Path.GetDirectoryName(path);

            if (!Directory.Exists(dir)) {
                DirectoryInfo di = Directory.CreateDirectory(dir);
                if (!di.Exists) {
                    return;
                }
            }

            //Debug.Log("Logger path: " + path);
            logger = Loggers.NewFileLogger("tag", path, TimeSpan.FromSeconds(30));
            write("sgffu.Log.Service::init()");
        }

        public static void write(string append)
        {
            //Debug.Log("sgffu.Log.Service: " + append);
            //logger.Log("tag", append);
        }
    }
}