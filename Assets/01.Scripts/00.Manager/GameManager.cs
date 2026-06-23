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
        public List<int> characterGrade = new();
        public List<int> weaponGrade = new();

        public PlayerData()
        {
            clearedStages.Clear();
            characterGrade.Clear();

            foreach (WorldNum world in Enum.GetValues(typeof(WorldNum)))
            {
                WorldData newWorld = new WorldData { worldNum = world };
            
                foreach (StageNum stage in Enum.GetValues(typeof(StageNum)))
                {
                    newWorld.stages.Add(new StageData { stageNum = stage, isCleared = false });
                }
            
                clearedStages.Add(newWorld);
            }

            for (int i = 0; i < 6; i++)
            {
                characterGrade.Add(0);
            }

            for (int i = 0; i < 7; i++)
            {
                weaponGrade.Add(0);
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
    public class SelectedData
    {
        public List<int> selectedCharacters = new() { 0, 1 };
        public int selectedWeapon;
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
        public PlayerData playerData;
        public SelectedData selectedData;
        public SoundData soundData;
        private float _sessionStartTime;

        public Action OnCoinChanged;

        protected override void Awake()
        {
            base.Awake();

            playerData = new PlayerData();
            selectedData = new SelectedData();
            soundData = new SoundData();
            
            _sessionStartTime = Time.time;
        }

        private void Start()
        {
            LoadGame();
            LoadSelected();
            LoadSound();
        }
        
        // 인게임 매니저로 이동 예정
        private void UpdatePlayTime()
        {
            playerData.playTime += Time.time - _sessionStartTime;
            _sessionStartTime = Time.time;
        }

        public void SaveGame()
        {
            UpdatePlayTime();
            SaveLoadManager.Instance.SaveData(playerData);
        }

        private void LoadGame()
        {
            SaveLoadManager.Instance.LoadData(playerData);
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

        public void SaveSelected()
        {
            SaveLoadManager.Instance.SaveData(selectedData);
        }

        public void LoadSelected()
        {
            SaveLoadManager.Instance.LoadData(selectedData);
        }

        public void AddAndUseCoin(int amount)
        {
            playerData.coin += amount;
            
            OnCoinChanged?.Invoke();
        }
    }
}
