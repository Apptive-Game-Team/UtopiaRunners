using System;
using System.Collections;
using UnityEngine;

namespace _01.Scripts._06.Weapon.RPG
{
    public class RpgController : WeaponController
    {
        [SerializeField] private float explosionRange = 5;
        
        private void Awake()
        {
            SetRpgAttackEffect();
        }

        private void SetRpgAttackEffect()
        {
            AddAttackEffect(int.MaxValue, (e, f) =>
            {
                GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

                foreach (GameObject enemy in enemies)
                {
                    if (Vector3.Distance(enemy.transform.position, transform.position) < explosionRange)
                    {
                        if (enemy.gameObject == e.gameObject)
                        {
                            continue;
                        }
                        enemy.GetComponent<EnemyHp>().TakeDamage(attackDamage * 2.5f / 5f);
                    }
                }
            });
        }
        
        protected override IEnumerator AutoAttack()
        {
            while (true)
            {
                if (GameObject.FindWithTag("Enemy") != null)
                {
                    AutoAttackProjectile projectileScript = SetAttackProjectile();
                    SetAttackEffect(projectileScript);
                }

                yield return new WaitForSeconds(weaponInfo.attackSpeed * 2f);
            }
        }

        protected override AutoAttackProjectile SetAttackProjectile()
        {
            GameObject projectile = Instantiate(attackPrefab, transform.position, Quaternion.identity);

            AutoAttackProjectile projectileScript = projectile.GetComponent<AutoAttackProjectile>();
                    
            if (projectileScript != null)
            {
                projectileScript.Init(attackDamage * 2.5f);
            }
            
            return projectileScript;
        }
    }
}
