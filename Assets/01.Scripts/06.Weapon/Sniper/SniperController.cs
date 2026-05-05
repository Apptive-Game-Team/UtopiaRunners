using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _01.Scripts._06.Weapon.Sniper
{
    public class SniperController : WeaponController
    {
        public bool isSkillUsed;
        
        protected override IEnumerator AutoAttack()
        {
            while (true)
            {
                if (GameObject.FindWithTag("Enemy") != null)
                {
                    GameObject projectile = Instantiate(attackPrefab, transform.position, Quaternion.identity);

                    SniperAutoAttackProjectile projectileScript = projectile.GetComponent<SniperAutoAttackProjectile>();
                    if (projectileScript != null)
                    {
                        projectileScript.Init(attackDamage);
                        projectileScript.SetCharacter(transform.parent.gameObject);
                        if (isSkillUsed)
                        {
                            projectileScript.isSkillUsed = true;
                            projectileScript.DelayedDestroy(5f);
                        }
                    }
                }

                yield return new WaitForSeconds(weaponInfo.attackSpeed);
            }
        }
    }
}
