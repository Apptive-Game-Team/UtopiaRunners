using UnityEngine;

namespace _01.Scripts._06.Weapon.PlasmaGun
{
    public class PlasmaGunController : WeaponController
    {
        [SerializeField] private float explosionRange = 5;
        
        private void Awake()
        {
            SetPlasmaGunAttackEffect();
        }

        private void SetPlasmaGunAttackEffect()
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
                        enemy.GetComponent<EnemyHp>().TakeDamage(attackDamage / 5f);
                    }
                }
            });
        }
    }
}
