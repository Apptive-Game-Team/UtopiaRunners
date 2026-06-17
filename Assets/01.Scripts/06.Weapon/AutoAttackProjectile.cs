using System;
using System.Collections.Generic;
using UnityEngine;

namespace _01.Scripts._06.Weapon
{
    public class AutoAttackProjectile : MonoBehaviour
    {
        [SerializeField] private float speed = 10f;
        private GameObject _targetEnemy;
        private Vector3 _targetDirection;
        
        protected Action<GameObject, float> OnHitEffects;
        protected float Damage;

        public void Init(float damage)
        {
            Damage = damage;
        }
        
        public void AddEffect(Action<GameObject, float> effect)
        {
            OnHitEffects += effect;
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
                ApplyHit(collision.gameObject);
                Destroy(gameObject);
            }
        }

        protected virtual void ApplyHit(GameObject enemy)
        {
            float damage = CalculateDamage();
            enemy.GetComponent<EnemyHp>()?.TakeDamage(damage);
            enemy.GetComponent<BossHp>()?.TakeDamage(damage);
            
            OnHitEffects?.Invoke(enemy, damage);
        }
        
        protected virtual float CalculateDamage() => Damage;
    }
}