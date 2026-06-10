using System.Collections.Generic;
using _01.Scripts._03.Data;
using _01.Scripts._04.UI;
using _01.Scripts._05.Utility;
using UnityEngine;

namespace _01.Scripts._00.Manager
{
    public enum WorldNum
    {
        World0,
        World1,
        World2,
        World3,
        World4,
    }

    public enum StageNum
    {
        Stage0,
        Stage1,
        Stage2,
        Stage3,
        Stage4,
        Stage5,
    }
    
    public class StageManager : SingletonObject<StageManager>
    {
        public WorldNum currentWorldNum;
        public StageNum currentStageNum;

        public WorldNum selectedWorldNum;
        public StageNum selectedStageNum;
        public WorldInfo selectedWorldInfo;

        public int selectedCharacterIdx;
        public List<int> selectedCharacters;
        public int selectedWeapon;

        protected override void Awake()
        {
            base.Awake();
            
            InitialSetting();
        }

        private void InitialSetting()
        {
            currentWorldNum = GameManager.Instance.playerData.currentWorld;
            currentStageNum = GameManager.Instance.playerData.currentStage;

            selectedCharacters = GameManager.Instance.selectedData.selectedCharacters;
            selectedWeapon = GameManager.Instance.selectedData.selectedWeapon;
        }

        public int GetSelectedCharacterIdx()
        {
            return selectedCharacters[selectedCharacterIdx];
        }
    }
}
