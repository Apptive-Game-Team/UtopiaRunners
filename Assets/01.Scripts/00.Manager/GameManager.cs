using System.Collections.Generic;
using _01.Scripts._05.Utility;
using System;
using UnityEngine;

namespace _01.Scripts._00.Manager
{
    [Serializable]
    public class PlayerData
    {
        public int coin = 0;
        public string currentWorld = "World1";
        public string currentStage = "Stage1";
        public float playTime = 0f;
        public Dictionary<string, Dictionary<string, bool>> ClearedStages = new();
    }
    
    public class GameManager : SingletonObject<GameManager>
    {
        public PlayerData PlayerData { get; private set; } = new();
        private float _sessionStartTime;

        protected override void Awake()
        {
            base.Awake();
            
            _sessionStartTime = Time.time;
        }

        public void AddCoin(int amount)
        {
            PlayerData.coin += amount;
        }

        public void SetCurrentWorld(string world)
        {
            PlayerData.currentWorld = world;
        }

        public void SetCurrentStage(string stage)
        {
            PlayerData.currentStage = stage;
        }
        
        // 인게임 매니저로 이동 예정
        private void UpdatePlayTime()
        {
            PlayerData.playTime += Time.time - _sessionStartTime;
            _sessionStartTime = Time.time;
        }

        public void SaveGame()
        {
            UpdatePlayTime();
            SaveLoadManager.Instance.SaveGame(PlayerData);
        }

        public void LoadGame()
        {
            SaveLoadManager.Instance.LoadGame(PlayerData);
        }
    }
}
