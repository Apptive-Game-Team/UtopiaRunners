using UnityEngine;

namespace _01.Scripts._06.Weapon
{
    public class AttackObject : MonoBehaviour
    {
        private float _damage;

        public void Init(float damage)
        {
            _damage = damage;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Enemy"))
            {
                collision.GetComponent<EnemyHp>().TakeDamage(_damage);
            }
        }
    }
}
