using System.Collections;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [Header("Components")]
    public WeaponData weaponData;
    public WeaponSkillBase skill;
    public PlayerInput input;

    public GameObject targetEnemy;
    public GameObject weaponSkillObj;

    private float skillCooldownTimer;

    private void Awake()
    {
        input = GetComponent<PlayerInput>();
        skill = GetComponent<WeaponSkillBase>();
    }

    private void Start()
    {
        weaponSkillObj = Instantiate(
            weaponData.weaponSkillPrefab, 
            transform
            );

        StartCoroutine(AutoAttack());
    }

    private void Update()
    {
        targetEnemy = FindNearestEnemy();

        if (skillCooldownTimer > 0f)
            skillCooldownTimer -= Time.deltaTime;

        if (input.skillPressed && skillCooldownTimer <= 0f)
            UseSkill();
    }

    private IEnumerator AutoAttack()
    {
        while (true)
        {
            if (targetEnemy != null)
            {
                GameObject projectile = Instantiate(
                    weaponData.autoAttackProjectile,
                    transform.position,
                    Quaternion.identity
                );

                AutoAttackProjectile projectileScript = projectile.GetComponent<AutoAttackProjectile>();
                if (projectileScript != null)
                    projectileScript.Init(targetEnemy.transform, weaponData.attackDamage);
            }

            yield return new WaitForSeconds(weaponData.attackSpeed);
        }
    }

    private void UseSkill()
    {
        skill.Activate();
    }

    private GameObject FindNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        GameObject nearestEnemy = null;
        float nearestDistance = float.MaxValue;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);

            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestEnemy = enemy;
            }
        }

        return nearestEnemy;
    }
}
