using System;
using System.IO;
using System.Text;
using UnityEngine;

namespace TestTask.Infrastructure.SaveLoad
{
    public static class SaveUtility
    {
        private const string FolderName = "saves";

        public static bool TryLoad<T>(string key, out T data)
        {
            return TryLoad(key, out data, out _);
        }

        public static bool TryLoad<T>(string key, out T data, out string error)
        {
            data = default;
            error = null;

            var path = GetPath(key);
            if (!File.Exists(path))
            {
                error = "not_found";
                return false;
            }

            string json;
            try
            {
                json = File.ReadAllText(path, Encoding.UTF8);
            }
            catch (Exception e)
            {
                error = "io_read_" + e.GetType().Name;
                return false;
            }

            if (string.IsNullOrWhiteSpace(json))
            {
                error = "empty";
                return false;
            }

            try
            {
                data = JsonUtility.FromJson<T>(json);
                return true;
            }
            catch (Exception e)
            {
                error = "json_" + e.GetType().Name;
                return false;
            }
        }

        public static bool TrySave<T>(string key, T data)
        {
            return TrySave(key, data, out _);
        }

        public static bool TrySave<T>(string key, T data, out string error)
        {
            error = null;

            var path = GetPath(key);
            var dir = Path.GetDirectoryName(path);
            if (!string.IsNullOrEmpty(dir))
                Directory.CreateDirectory(dir);

            string json;
            try
            {
                json = JsonUtility.ToJson(data, prettyPrint: false);
            }
            catch (Exception e)
            {
                error = "json_" + e.GetType().Name;
                return false;
            }

            var tmp = path + ".tmp";
            try
            {
                File.WriteAllText(tmp, json, new UTF8Encoding(false));
                if (File.Exists(path))
                    File.Replace(tmp, path, destinationBackupFileName: null);
                else
                    File.Move(tmp, path);
                return true;
            }
            catch (Exception e)
            {
                error = "io_write_" + e.GetType().Name;
                try
                {
                    if (File.Exists(tmp))
                        File.Delete(tmp);
                }
                catch { }
                return false;
            }
        }

        private static string GetPath(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentException("Key is empty.", nameof(key));

            foreach (var c in Path.GetInvalidFileNameChars())
                key = key.Replace(c, '_');

            return Path.Combine(Application.persistentDataPath, FolderName, key + ".json");
        }
    }
}

