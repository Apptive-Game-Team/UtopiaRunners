using System;
using UnityEngine;

namespace _01.Scripts._06.Weapon
{
    public class AutoAttackProjectile : MonoBehaviour
    {
        [SerializeField] private float speed = 10f;

        // 기존 기능: 가장 가까운 적을 찾아서 그 방향으로 발사
        // private GameObject _targetEnemy;
        // private Vector3 _targetDirection;

        // 변경 기능: 그냥 오른쪽으로 일직선 이동
        private Vector3 _moveDirection = Vector3.right;

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
            // 기존 기능: 가장 가까운 적을 찾아서 그 방향으로 이동
            /*
            if (_targetEnemy == null)
            {
                _targetEnemy = FindNearestEnemy();

                if (_targetEnemy == null)
                {
                    Destroy(gameObject);
                    return;
                }

                _targetDirection =
                    (_targetEnemy.transform.position - transform.position).normalized;
            }

            transform.Translate(_targetDirection * (speed * Time.deltaTime));
            */

            // 변경 기능: 오른쪽으로 일직선 이동
            transform.Translate(
                _moveDirection * (speed * Time.deltaTime),
                Space.World
            );
        }

        // 기존 기능: 가장 가까운 적 찾기
        /*
        private GameObject FindNearestEnemy()
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

            GameObject nearestEnemy = null;
            float nearestDistance = float.MaxValue;

            foreach (GameObject enemy in enemies)
            {
                float distance =
                    Vector2.Distance(transform.position, enemy.transform.position);

                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestEnemy = enemy;
                }
            }

            return nearestEnemy;
        }
        */

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

        protected virtual float CalculateDamage()
        {
            return Damage;
        }
    }
}