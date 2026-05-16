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
        
        protected override float CalculateDamage()
        {
            float dist = Vector3.Distance(transform.position, _character.transform.position);
            float t = Mathf.InverseLerp(5f, 10f, dist);
            float multiplier = Mathf.Lerp(1f, 1.5f, t);
        
            if (!isSkillUsed)
            {
                return Damage * multiplier;
            }
            
            return _isFirstTarget ? (Damage * multiplier * 2) : (Damage * multiplier / 2);
        }

        public void DelayedDestroy(float time)
        {
            Destroy(gameObject, time);
        }
        
        protected override void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Enemy"))
            {
                ApplyHit(collision.gameObject);

                if (!isSkillUsed)
                {
                    Destroy(gameObject);
                }
                else if (_isFirstTarget)
                {
                    _isFirstTarget = false;
                }
            }
        }
    }
}
