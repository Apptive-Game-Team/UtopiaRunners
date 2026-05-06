using System.Collections.Generic;
using UnityEngine;

namespace _01.Scripts._06.Weapon
{
    public class AttackObject : MonoBehaviour
    {
        private float _damage;
        private float _continuousDamage;
        private bool _oneTimeAttack;
        private bool _continuousAttack;
        private float _continuousAttackDelay;
        
        private float _initialAttackDuration = 0.15f; 
        private float _spawnTime;

        private Dictionary<Collider2D, float> _lastAttackTimes = new();

        public void Init(float damage, bool isOneTime = true, 
            bool isContinuous = false, float continuousDamage = 0f, float attackDelay = 0f, float attackDuration = 0f)
        {
            _damage = damage;
            _oneTimeAttack = isOneTime;
            _continuousAttack = isContinuous;
            _continuousDamage = continuousDamage;
            _continuousAttackDelay = attackDelay;
            
            _spawnTime = Time.time;

            if (_continuousAttack)
            {
                Destroy(gameObject, attackDuration);
            }
            else if (_oneTimeAttack)
            {
                Destroy(gameObject, _initialAttackDuration);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Enemy") && _oneTimeAttack)
            {
                if (Time.time <= _spawnTime + _initialAttackDuration)
                {
                    if (collision.TryGetComponent(out EnemyHp enemyHp))
                    {
                        enemyHp.TakeDamage(_damage);
                    }
                }
            }
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.CompareTag("Enemy") && _continuousAttack)
            {
                _lastAttackTimes.TryAdd(collision, 0f);

                if (Time.time >= _lastAttackTimes[collision] + _continuousAttackDelay)
                {
                    if (collision.TryGetComponent(out EnemyHp enemyHp))
                    {
                        enemyHp.TakeDamage(_continuousDamage);
                        _lastAttackTimes[collision] = Time.time;
                    }
                }
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            _lastAttackTimes.Remove(collision);
        }
    }
}