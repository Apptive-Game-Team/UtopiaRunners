using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewStageWaveData", menuName = "Data/Stage Wave")]
public class StageWaveData : ScriptableObject
{
    public List<WaveTimelineData> waveTimelines = new List<WaveTimelineData>();
}