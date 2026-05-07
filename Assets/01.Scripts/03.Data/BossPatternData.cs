using UnityEngine;

[System.Serializable]
public class BossPatternData
{
    public string patternName;

    public GameObject patternPrefab;
    public EnemyAttackType attackType;
    public float attackDamage;
    public float cooldown;
}