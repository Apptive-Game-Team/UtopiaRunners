using UnityEngine;

[CreateAssetMenu(fileName = "NewWeaponData", menuName = "Data/Weapon")]
public class WeaponData : ScriptableObject
{
    [Header("Name")]
    public string weaponName;

    [Header("Auto Attack")]
    public GameObject autoAttackProjectile;
    public float attackDamage = 1f;
    public float attackSpeed = 0.5f;

    [Header("Skill")]
    public GameObject weaponSkillPrefab;
    public float cooldown = 5f;
}
