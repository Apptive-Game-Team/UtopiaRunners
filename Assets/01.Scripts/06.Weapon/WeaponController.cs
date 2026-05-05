using System;
using System.Collections;
using _01.Scripts._00.Manager;
using _01.Scripts._04.UI;
using UnityEngine;
using UnityEngine.Serialization;

public class WeaponController : MonoBehaviour
{
    [Header("Components")]
    public WeaponInfo weaponInfo;
    public int weaponId;
    public WeaponSkillBase skill;
    public GameObject weaponSkillObj;
    
    private float _skillCooldownTimer;

    private void Start()
    {
        InputManager.AddListener(ActionCode.Skill, InputType.Down, SkillInput);
    }

    public void Initialize()
    {
        weaponSkillObj = Instantiate(
            weaponInfo.skillPrefab, 
            transform
        );

        skill = weaponSkillObj.GetComponent<WeaponSkillBase>();

        if (skill != null)
        {
            skill.Init(this);
        }

        StartCoroutine(AutoAttack());
    }

    private void SkillInput()
    {
        if (Time.time - _skillCooldownTimer > weaponInfo.coolTime)
        {
            skill.Activate();
            _skillCooldownTimer = Time.time;
        }
    }

    private IEnumerator AutoAttack()
    {
        while (true)
        {
            if (GameObject.FindWithTag("Enemy") != null)
            {
                GameObject projectile = Instantiate(
                    weaponInfo.attackPrefab,
                    transform.position,
                    Quaternion.identity
                );

                AutoAttackProjectile projectileScript = projectile.GetComponent<AutoAttackProjectile>();
                if (projectileScript != null)
                    projectileScript.Init(weaponInfo.apList[0]);
            }

            yield return new WaitForSeconds(weaponInfo.attackSpeed);
        }
    }

    private void OnDestroy()
    {
        InputManager.RemoveListener(ActionCode.Skill, InputType.Down, SkillInput);
    }
}
