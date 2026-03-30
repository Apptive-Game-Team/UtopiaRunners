using System.IO;
using _01.Scripts._05.Utility;
using UnityEngine;

namespace _01.Scripts._00.Manager
{
    public class SaveLoadManager : SingletonObject<SaveLoadManager>
    {
        private string _savePath;
        
        protected override void Awake()
        {
            base.Awake();

            _savePath = Application.persistentDataPath;
        }

        public void SaveGame(PlayerData playerData)
        {
            PlayerData saveData = new PlayerData()
            {
                coin = playerData.coin,
                currentWorld = playerData.currentWorld,
                currentStage = playerData.currentStage,
                playTime = playerData.playTime,
                ClearedStages = playerData.ClearedStages
            };
            
            string path = Path.Combine(_savePath, "save.json");
            string json = JsonUtility.ToJson(saveData, true);
            File.WriteAllText(path, json);
        }

        public void LoadGame(PlayerData playerData)
        {
            string path = Path.Combine(_savePath, $"save.json");

            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                PlayerData data = JsonUtility.FromJson<PlayerData>(json);

                playerData.coin = data.coin;
                playerData.currentWorld = data.currentWorld;
                playerData.currentStage = data.currentStage;
                playerData.playTime = data.playTime;
                playerData.ClearedStages = data.ClearedStages;
            }
        }
    }
}
