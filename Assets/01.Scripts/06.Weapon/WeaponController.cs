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
        private float _currentCooldown;

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

        private void Update()
        {
            if (_currentCooldown > 0)
            {
                _currentCooldown -= Time.deltaTime;
            }
        }

        private void SkillInput()
        {
            if (_currentCooldown <= 0)
            {
                _skill.Activate();
                _currentCooldown = weaponInfo.coolTime;
            }
        }
        
        public void ReduceCooldown(float percent)
        {
            float reduction = weaponInfo.coolTime * (percent / 100f);
            _currentCooldown -= reduction;

            if (_currentCooldown < 0)
            {
                _currentCooldown = 0;
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
