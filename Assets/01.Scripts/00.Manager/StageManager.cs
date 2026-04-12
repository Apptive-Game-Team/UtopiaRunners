using _01.Scripts._03.Data;
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

        protected override void Awake()
        {
            base.Awake();
            
            InitialSetting();
        }

        private void InitialSetting()
        {
            currentWorldNum = GameManager.Instance.playData.currentWorld;
            currentStageNum = GameManager.Instance.playData.currentStage;
        }
        
        
    }
}
