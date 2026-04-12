using System.Collections.Generic;
using _01.Scripts._05.Utility;
using System;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Serialization;

namespace _01.Scripts._00.Manager
{
    [Serializable]
    public class PlayerData
    {
        public int coin = 0;
        public WorldNum currentWorld = WorldNum.World0;
        public StageNum currentStage = StageNum.Stage0;
        public float playTime = 0f;
        public List<WorldData> clearedStages = new();

        public PlayerData()
        {
            clearedStages.Clear();

            foreach (WorldNum world in Enum.GetValues(typeof(WorldNum)))
            {
                WorldData newWorld = new WorldData { worldNum = world };
            
                foreach (StageNum stage in Enum.GetValues(typeof(StageNum)))
                {
                    newWorld.stages.Add(new StageData { stageNum = stage, isCleared = false });
                }
            
                clearedStages.Add(newWorld);
            }
        }
    }

    [Serializable]
    public class WorldData
    {
        public WorldNum worldNum;
        public List<StageData> stages = new();
    }

    [Serializable]
    public class StageData
    {
        public StageNum stageNum;
        public bool isCleared;
    }

    [Serializable]
    public class SoundData
    {
        public float masterVolume = 0.5f;
        public float bgmVolume = 0.5f;
        public float sfxVolume = 0.5f;
    }
    
    public class GameManager : SingletonObject<GameManager>
    {
        public PlayerData playData;
        public SoundData soundData;
        private float _sessionStartTime;

        public Action OnCoinChanged;

        protected override void Awake()
        {
            base.Awake();

            playData = new PlayerData();
            soundData = new SoundData();
            _sessionStartTime = Time.time;
        }

        private void Start()
        {
            LoadGame();
            LoadSound();
        }
        
        // 인게임 매니저로 이동 예정
        private void UpdatePlayTime()
        {
            playData.playTime += Time.time - _sessionStartTime;
            _sessionStartTime = Time.time;
        }

        public void SaveGame()
        {
            UpdatePlayTime();
            SaveLoadManager.Instance.SaveData(playData);
        }

        private void LoadGame()
        {
            SaveLoadManager.Instance.LoadData(playData);
        }

        public void SaveSound()
        {
            SoundManager.Instance.SaveSoundData(soundData);
            SaveLoadManager.Instance.SaveData(soundData);
        }

        private void LoadSound()
        {
            SaveLoadManager.Instance.LoadData(soundData);
            SoundManager.Instance.LoadSoundData(soundData);
        }

        public void AddAndUseCoin(int amount)
        {
            playData.coin += amount;
            
            OnCoinChanged?.Invoke();
        }
    }
}
