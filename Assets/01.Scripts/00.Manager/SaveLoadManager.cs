using System.Collections.Generic;
using System.IO;
using _01.Scripts._05.Utility;
using UnityEngine;

namespace _01.Scripts._00.Manager
{
    public class SaveLoadManager : SingletonObject<SaveLoadManager>
    {
        private string GetPath<T>()
        {
            string fileName = typeof(T).Name;

            #if UNITY_EDITOR
                return Path.Combine(Application.dataPath, $"Data/{fileName}.json");
            #else
                return Path.Combine(Application.persistentDataPath, $"{fileName}.json");
            #endif
        }

        public void SaveData<T>(T data)
        {
            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(GetPath<T>(), json);
            
            #if UNITY_EDITOR
                UnityEditor.AssetDatabase.Refresh();
            #endif
        }

        public void LoadData<T>(T data)
        {
            string path = GetPath<T>();
            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                JsonUtility.FromJsonOverwrite(json, data);
            }
        }
    }
}
