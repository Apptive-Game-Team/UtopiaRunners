using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewWaveTimelineData", menuName = "Data/Wave Timeline")]
public class WaveTimelineData : ScriptableObject
{
    public float baseTime;
    public List<SpawnEventData> spawnEvents = new List<SpawnEventData>();
}