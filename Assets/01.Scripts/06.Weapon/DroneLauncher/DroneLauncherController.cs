using System.Collections;
using UnityEngine;

namespace _01.Scripts._06.Weapon.DroneLauncher
{
    public class DroneLauncherController : WeaponController
    {
        [SerializeField] private int enhancedAttackCount;
        private int _attackCount;
        
        protected override IEnumerator AutoAttack()
        {
            while (true)
            {
                if (GameObject.FindWithTag("Enemy") != null)
                {
                    GameObject projectile = Instantiate(attackPrefab, transform.position, Quaternion.identity);

                    _attackCount++;

                    AutoAttackProjectile projectileScript = projectile.GetComponent<AutoAttackProjectile>();
                    if (projectileScript != null)
                    {
                        bool isEnhanced = (_attackCount == enhancedAttackCount);
                        projectileScript.Init(weaponInfo.apList[0] * (isEnhanced ? 2 : 1));
                        if (isEnhanced)
                        {
                            projectile.GetComponent<SpriteRenderer>().color = Color.red;
                            _attackCount = 0;
                        }
                    }
                }

                yield return new WaitForSeconds(weaponInfo.attackSpeed);
            }
        }
    }
}
