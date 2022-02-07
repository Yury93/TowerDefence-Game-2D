using System;
using System.IO;
using UnityEngine;

namespace TowerDeffense
{
    [Serializable]
    public class Saver<T>
    {
        public static void TryLoad(string fileName, ref T data)
        {
            var path = FileHandler.Path(fileName);
            Debug.Log(path);
            if (File.Exists(path))
            {
                Debug.Log($"loading from: {FileHandler.Path(fileName)}");
                var dataString = File.ReadAllText(path);
                var saver = JsonUtility.FromJson<Saver<T>>(dataString);
                data = saver.Data;
            }
            else
            {
                Debug.Log($"Not file at path");
            }
        }

        public T Data;
        public static void Save(string fileName, T data)
        {
            Debug.Log($"saver to {FileHandler.Path(fileName)}");
            var wrapper = new Saver<T> { Data = data };
            var dataString = JsonUtility.ToJson(wrapper);
            File.WriteAllText(FileHandler.Path(fileName), dataString);
        }
    }
    public static class FileHandler
        {
        public static string Path(string fileName)
        {
            return $"{Application.persistentDataPath}/{fileName}";
        }
        public static void Reset(string fileName)
        {
            var path = Path(fileName);
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        public static bool HasFile(string fileName)
        {
            return File.Exists(Path(fileName));
        }
    }
}