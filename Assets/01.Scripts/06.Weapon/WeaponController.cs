using System.Collections;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [Header("Components")]
    public WeaponData weaponData;
    public PlayerInput input;

    public GameObject targetEnemy;
    public GameObject weaponSkillObj;

    private float skillCooldownTimer;

    private void Awake()
    {
        input = GetComponent<PlayerInput>();
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
        if (skillCooldownTimer > 0f)
            skillCooldownTimer -= Time.deltaTime;

        if (input.skillPressed)
            UseSkill();

        targetEnemy = FindNearestEnemy();
    }

    private IEnumerator AutoAttack()
    {
        while (true)
        {
            targetEnemy = FindNearestEnemy();

            if (targetEnemy != null)
            {
                Instantiate(
                    weaponData.autoAttackProjectile,
                    transform.position,
                    Quaternion.identity
                );
            }

            //Projectile projectileScript = projectile.GetComponent<Projectile>();
            //if (projectileScript != null)
            //    projectileScript.Init(targetEnemy.transform);

            yield return new WaitForSeconds(weaponData.attackSpeed);
        }

    }

    private void UseSkill()
    {
        weaponSkillObj.SetActive(true);
    }

    private GameObject FindNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        GameObject nearestEnemy = null;
        float nearestDistance = 20f;

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
