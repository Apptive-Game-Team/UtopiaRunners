using System.Collections;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [Header("Components")]
    public WeaponData weaponData;
    public WeaponSkillBase skill;
    public PlayerInput input;

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

        skill = weaponSkillObj.GetComponent<WeaponSkillBase>();

        if (skill != null)
        {
            skill.Init(this);
        }

        StartCoroutine(AutoAttack());
    }

    private void Update()
    {
        if (skillCooldownTimer > 0f && !skill.isSkilling)
            skillCooldownTimer -= Time.deltaTime;

        if (input.skillPressed && skillCooldownTimer <= 0f)
        {
            UseSkill();
            skillCooldownTimer = weaponData.cooldown;
        }
    }

    private IEnumerator AutoAttack()
    {
        while (true)
        {
            if (GameObject.FindWithTag("Enemy") != null)
            {
                GameObject projectile = Instantiate(
                    weaponData.autoAttackProjectile,
                    transform.position,
                    Quaternion.identity
                );

                AutoAttackProjectile projectileScript = projectile.GetComponent<AutoAttackProjectile>();
                if (projectileScript != null)
                    projectileScript.Init(weaponData.attackDamage);
            }

            yield return new WaitForSeconds(weaponData.attackSpeed);
        }
    }

    private void UseSkill()
    {
        skill.Activate();
    }
}
