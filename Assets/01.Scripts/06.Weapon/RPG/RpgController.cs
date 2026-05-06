using System.Collections;
using UnityEngine;

namespace _01.Scripts._06.Weapon.RPG
{
    public class RpgController : WeaponController
    {
        protected override IEnumerator AutoAttack()
        {
            while (true)
            {
                if (GameObject.FindWithTag("Enemy") != null)
                {
                    GameObject projectile = Instantiate(attackPrefab, transform.position, Quaternion.identity);

                    AutoAttackProjectile projectileScript = projectile.GetComponent<AutoAttackProjectile>();
                    if (projectileScript != null)
                    {
                        projectileScript.Init(attackDamage * 2.5f);
                    }
                }

                yield return new WaitForSeconds(weaponInfo.attackSpeed * 2f);
            }
        }
    }
}
