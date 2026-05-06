using System.Collections;
using _01.Scripts._00.Manager;
using _01.Scripts._04.UI;
using UnityEngine;

namespace _01.Scripts._06.Weapon
{
    public class WeaponController : MonoBehaviour
    {
        [Header("Components")]
        public WeaponInfo weaponInfo;
        public int weaponId;
        public GameObject attackPrefab;
        public GameObject skillPrefab;
        public float attackDamage;
        public float skillDamage;
    
        private WeaponSkillBase _skill;
    
        private float _skillCooldownTimer;

        protected virtual void Start()
        {
            InputManager.AddListener(ActionCode.Skill, InputType.Down, SkillInput);
            attackDamage = weaponInfo.apList[0];
            skillDamage = weaponInfo.skillValue[0];
        }

        public void Initialize()
        {
            _skill = Instantiate(skillPrefab, transform).GetComponent<WeaponSkillBase>();

            if (_skill != null)
            {
                _skill.Init(this);
            }

            StartCoroutine(AutoAttack());
        }

        private void SkillInput()
        {
            if (Time.time - _skillCooldownTimer > weaponInfo.coolTime)
            {
                _skill.Activate();
                _skillCooldownTimer = Time.time;
            }
        }

        protected virtual IEnumerator AutoAttack()
        {
            while (true)
            {
                if (GameObject.FindWithTag("Enemy") != null)
                {
                    GameObject projectile = Instantiate(attackPrefab, transform.position, Quaternion.identity);

                    AutoAttackProjectile projectileScript = projectile.GetComponent<AutoAttackProjectile>();
                    if (projectileScript != null)
                    {
                        projectileScript.Init(attackDamage);
                    }
                }

                yield return new WaitForSeconds(weaponInfo.attackSpeed);
            }
        }

        private void OnDestroy()
        {
            InputManager.RemoveListener(ActionCode.Skill, InputType.Down, SkillInput);
        }
    }
}
