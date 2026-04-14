using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewWaveSetData", menuName = "Data/WaveSet")]
public class WaveSet : ScriptableObject
{
    public List<WaveData> enemyWaves = new List<WaveData>();
    public List<WaveData> obstacleWaves = new List<WaveData>();
}