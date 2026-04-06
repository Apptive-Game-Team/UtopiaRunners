using System.IO;
using _01.Scripts._05.Utility;
using UnityEngine;

namespace _01.Scripts._00.Manager
{
    public class SaveLoadManager : SingletonObject<SaveLoadManager>
    {
        private string _playerDataSavePath;
        private string _soundDataSavePath;
        
        protected override void Awake()
        {
            base.Awake();

            #if UNITY_EDITOR
                _playerDataSavePath = Path.Combine(Application.dataPath, "Data/PlayerData.json");
                _soundDataSavePath = Path.Combine(Application.dataPath, "Data/SoundData.json");
            #else
                _playerDataSavePath = Path.Combine(Application.persistentDataPath, "PlayerDataSave.json");
                _soundDataSavePath = Path.Combine(Application.persistentDataPath, "SoundDataSave.json");
            #endif
        }

        public void SaveGame(PlayerData playerData)
        {
            string json = JsonUtility.ToJson(playerData, true);
            File.WriteAllText(_playerDataSavePath, json);
            
            #if UNITY_EDITOR
                UnityEditor.AssetDatabase.Refresh();
            #endif
        }

        public void SaveSound(SoundData soundData)
        {
            string json = JsonUtility.ToJson(soundData, true);
            File.WriteAllText(_soundDataSavePath, json);
            
            #if UNITY_EDITOR
                UnityEditor.AssetDatabase.Refresh();
            #endif
        }

        public void LoadGame(PlayerData playerData)
        {
            if (File.Exists(_playerDataSavePath))
            {
                string json = File.ReadAllText(_playerDataSavePath);
                JsonUtility.FromJsonOverwrite(json, playerData);
            }
        }

        public SoundData LoadSound()
        {
            SoundData soundData = new SoundData();
            
            if (File.Exists(_soundDataSavePath))
            {
                string json = File.ReadAllText(_soundDataSavePath);
                JsonUtility.FromJsonOverwrite(json, soundData);
            }

            return soundData;
        }
    }
}
