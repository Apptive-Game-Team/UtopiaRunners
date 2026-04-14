using UnityEngine;

public enum WaveType
{
    Enemy,
    Obstacle
}

[CreateAssetMenu(fileName = "NewWaveData", menuName = "Data/Wave")]
public class WaveData : ScriptableObject
{
    public float startTime;
    public WaveType waveType;

    public GameObject prefab;
    public int spawnCount;
    public float spawnInterval;
}