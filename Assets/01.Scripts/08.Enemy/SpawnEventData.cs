using UnityEngine;

[System.Serializable]
public class SpawnEventData
{
    public GameObject prefab;
    public EnemyLane lane;
    public float delayFromBaseTime;
}