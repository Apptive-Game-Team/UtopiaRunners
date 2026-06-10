using System;
using System.Collections.Generic;
using _01.Scripts._00.Manager;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Stage Wave Database")]
public class StageWaveDatabase : ScriptableObject
{
    [SerializeField] private List<StageWaveInfo> stageWaveInfos = new List<StageWaveInfo>();

    public StageWaveData GetWaveData(WorldNum worldNum, StageNum stageNum)
    {
        foreach (StageWaveInfo info in stageWaveInfos)
        {
            if (info.worldNum == worldNum && info.stageNum == stageNum)
            {
                return info.stageWaveData;
            }
        }

        return null;
    }
}

[Serializable]
public class StageWaveInfo
{
    public WorldNum worldNum;
    public StageNum stageNum;
    public StageWaveData stageWaveData;
}