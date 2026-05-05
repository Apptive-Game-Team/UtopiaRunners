using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace _01.Scripts._06.Weapon.Sniper
{
    public class SniperAutoAttackProjectile : AutoAttackProjectile
    {
        private GameObject _character;
        public bool isSkillUsed;
        private bool _isFirstTarget = true;

        public void SetCharacter(GameObject character)
        {
            _character = character;
        }

        private float MultipliedDamage()
        {
            float t = Mathf.InverseLerp(5f, 10f, Vector3.Distance(transform.position, _character.transform.position));
            return Mathf.Lerp(1f, 1.5f, t);
        }

        public void DelayedDestroy(float time)
        {
            Destroy(gameObject, time);
        }
        
        protected override void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Enemy"))
            {
                if (!isSkillUsed)
                {
                    collision.GetComponent<EnemyHp>().TakeDamage(Damage * MultipliedDamage());
                    Destroy(gameObject);
                }
                else
                {
                    if (_isFirstTarget)
                    {
                        collision.GetComponent<EnemyHp>().TakeDamage(Damage * MultipliedDamage() * 2);
                        _isFirstTarget = false;
                    }
                    else
                    {
                        collision.GetComponent<EnemyHp>().TakeDamage(Damage * MultipliedDamage() / 2);
                    }
                }
            }
        }
    }
}
