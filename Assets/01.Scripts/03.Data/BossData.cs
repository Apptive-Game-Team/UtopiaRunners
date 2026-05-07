using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NewBossData", menuName = "Data/Boss")]

public class BossData : ScriptableObject
{
    [Header("Name")]
    public string enemyName;

    [Header("Stats")]
    public float healthPoint;

    [Header("Patterns")]
    public List<BossPatternData> patterns = new List<BossPatternData>(); 
}