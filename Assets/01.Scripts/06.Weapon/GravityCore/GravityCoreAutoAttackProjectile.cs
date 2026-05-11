using UnityEngine;

namespace _01.Scripts._06.Weapon.GravityCore
{
    public class GravityCoreAutoAttackProjectile : AutoAttackProjectile
    {
        protected override void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Enemy"))
            {
                ApplyHit(collision.gameObject);
                collision.GetComponent<Rigidbody2D>().AddForce((collision.transform.position - transform.position).normalized * 7f, ForceMode2D.Impulse);
                Destroy(gameObject);
            }
        }
    }
}
