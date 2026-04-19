using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyData", menuName = "Data/Enemy")]
public class EnemyData : ScriptableObject
{
    [Header("Name")]
    public string enemyName;

    [Header("Stats")]
    public float attackDamage;
    public float healthPoint;

    [Header("Pattern")]
    public GameObject enemyPatternPrefab;
    public float cooldown;
}
