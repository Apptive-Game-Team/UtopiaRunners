using UnityEngine;

namespace _01.Scripts._06.Weapon.RPG
{
    public class RpgAutoAttackProjectile : AutoAttackProjectile
    {
        [SerializeField] private float explosionRange;
        
        protected override void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Enemy"))
            {
                collision.GetComponent<EnemyHp>().TakeDamage(Damage);
                
                GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

                foreach (GameObject enemy in enemies)
                {
                    if (Vector3.Distance(enemy.transform.position, transform.position) < explosionRange)
                    {
                        if (enemy.gameObject == collision.gameObject)
                        {
                            continue;
                        }
                        enemy.GetComponent<EnemyHp>().TakeDamage(Damage / 5);
                    }
                }
                
                Destroy(gameObject);
            }
        }
    }
}
