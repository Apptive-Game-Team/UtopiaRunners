using UnityEngine;

namespace _01.Scripts._06.Weapon
{
    public class AutoAttackProjectile : MonoBehaviour
    {
        [SerializeField] private float speed = 10f;
        private GameObject _targetEnemy;
        private Vector3 _targetDirection;
        
        protected float Damage;

        public void Init(float damage)
        {
            Damage = damage;
        }

        private void Update()
        {
            if (_targetEnemy == null)
            {
                _targetEnemy = FindNearestEnemy();

                if (_targetEnemy == null)
                {
                    Destroy(gameObject);
                    return;
                }
                
                _targetDirection = (_targetEnemy.transform.position - transform.position).normalized;
            }
            
            transform.Translate(_targetDirection * (speed * Time.deltaTime));
        }
    
        private GameObject FindNearestEnemy()
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

            GameObject nearestEnemy = null;
            float nearestDistance = float.MaxValue;

            foreach (GameObject enemy in enemies)
            {
                float distance = Vector2.Distance(transform.position, enemy.transform.position);

                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestEnemy = enemy;
                }
            }

            return nearestEnemy;
        }

        protected virtual void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Enemy"))
            {
                collision.GetComponent<EnemyHp>().TakeDamage(Damage);
                Destroy(gameObject);
            }
        }
    }
}