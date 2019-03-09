using System.IO;
using UnityEngine;
using StrOpe = StringOperationUtil.OptimizedStringOperation;

namespace Filesystem {
    
    /// <summary>
    /// ファイル操作クラス
    /// </summary>
    public class File
    {

        public string filename = "";

        public string path = "";


        //public string default_filename = "file.txt";



        public File(string filename = "", string default_content = "")
        {
            this.filename = filename;
            if (filename.Length < 1) { return; }
            this.reset(filename, default_content);
        }



        /// <summary>
        /// クラスを初期化する。具体的にはファイルのパスを設定する。
        /// </summary>
        /// <returns>
        /// 初期化の成否。失敗時にはディレクトリが生成できていない
        /// </returns>
        public bool reset(string filename, string default_content)
        {
            this.filename = filename;
            Debug.Log("Filesystem.File.reset() : filename : " + filename);
            //if (filename.Length < 1) { filename = this.default_filename; }
            if (filename.Length < 1) return true;
            path = StrOpe.i + Application.dataPath + filename;
            Debug.Log("Filesystem.File.reset() : path : " + path);
            //Debug.Log("Terrain Texture2D filepath 2: " + this.path);
            string dir = Path.GetDirectoryName(path);
            Debug.Log("Filesystem.File.reset() : dir : " + dir);
/*
            Debug.Log("path: " + path);
            Debug.Log("dir: " + dir);
            Debug.Log("Directory.Exists(dir): " + Directory.Exists(dir));
*/
            if (!Directory.Exists(dir)) {
                DirectoryInfo di = Directory.CreateDirectory(dir);

                if (!di.Exists) {
                    return false;
                }
            }

            if (!System.IO.File.Exists(path) && 0 < default_content.Length) {
                Debug.Log("Filesystem.File.reset() : default_content : " + default_content);

                using (StreamWriter sw = System.IO.File.CreateText(path))
                {
                    sw.WriteLine(default_content);
                }
            }

            return true;
        }

        /// <summary>
        /// ファイルの存在有無を確認
        /// </summary>
        /// <returns>
        /// ファイルの存在有無（true：ファイル有、false：ファイル無）
        /// </returns>
        public bool exist(string filename)
        {
            string path = StrOpe.i + Application.dataPath + "/" + filename;
            Dir.create(path);
            //Debug.Log("exist: path: " + path);
            return System.IO.File.Exists(path);
        }

        public bool resourceExist(string filename)
        {
            string path = StrOpe.i + Application.dataPath + "/Resources/" + filename;
            //Debug.Log("resource_exist: path: " + path);
            return System.IO.File.Exists(path);
        }

        /// <summary>
        /// ファイルにテキストを書き込む
        /// </summary>
        public bool write(string text, string filename = "", append_type type = append_type.Append)
        {
            if (path == "") {
                if (!reset(filename, "")) {
                    return false;
                }
            }

            if (type == append_type.None) {
                using (StreamWriter sw = System.IO.File.CreateText(path))
                {
                    sw.WriteLine(text + "\n");
                }	
            } else {
                System.IO.File.AppendAllText(path, text + "\n");
            }

            return true;
        }

        public enum append_type {
            None,
            Append,
        }

        /// <summary>
        /// ファイルにfloatの二重配列を書き込む
        /// </summary>
        public bool writeFloat2(float[,] data, string filename = "")
        {
            if (path == "") {
                if (!reset(filename, "")) {
                    return false;
                }
            }

            int x = 0;
            int y = 0;
            int xMax = data.GetLength(0);
            int yMax = data.GetLength(1);
            string write_text = "";

            for (; x < xMax; x += 1) {
                for (; y < yMax; y += 1) {
                    write_text += (data[x, y] + ", ");
                }

                write_text += "\n";
            }

            System.IO.File.AppendAllText(path, write_text);
            return true;
        }



        /// <summary>
        /// ファイルからデータをstringとして読み込み
        /// </summary>
        public string read(string filename = "")
        {
            if (path == "" && filename.Length > 0) {
                if (!reset(filename, "")) {
                    return "";
                }
            }

            if (filename.Length > 0 && !exist(filename)) return "";
            if (path.Length <= 0) return "";
            return System.IO.File.ReadAllText(path);
        }

        /// <summary>
        /// ファイルからデータをstringの配列として読み込み
        /// </summary>
        public string[] readLines(string filename = "")
        {
            if (path == "" && filename.Length > 0) {
                if (!reset(filename, "")) {
                    return new string[0];
                }
            }

            if (!exist(filename)) return new string[0];
            return System.IO.File.ReadAllLines(path);
        }

        /// <summary>
        /// ファイルからデータをbyte配列として読み込み
        /// </summary>
        public byte[] readBytes(string filename = "")
        {
            if (filename.Length == 0 && 0 < this.filename.Length) {
                filename = this.filename;
            }

            if (path == "" && filename.Length > 0) {
                if (!reset(filename, "")) {
                    return new byte[0];
                }
            }

            if (!exist(filename)) return new byte[0];
            return System.IO.File.ReadAllBytes(path);
        }

    }

}