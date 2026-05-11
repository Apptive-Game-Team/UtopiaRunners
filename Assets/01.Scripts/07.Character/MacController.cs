using System.Collections;
using UnityEngine;

namespace _01.Scripts._07.Character
{
    public class MacController : PlayerController
    {
        [Header("Shield Skill Settings")]
        [SerializeField] private float shieldGenDelay = 5f;
        [SerializeField] private GameObject shieldPrefab;

        private float _originDamage;
        //private bool _hasShield;
        private Coroutine _shieldCoroutine;

        protected override void Start()
        {
            base.Start();
            _originDamage = damage;
        }

        private void OnEnable()
        {
            ResetShieldTimer();
        }

        private void OnDisable()
        {
            if (_shieldCoroutine != null)
            {
                StopCoroutine(_shieldCoroutine);
            }
        }
        

        private void ResetShieldTimer()
        {
            if (_shieldCoroutine != null)
            {
                StopCoroutine(_shieldCoroutine);
            }
            _shieldCoroutine = StartCoroutine(ShieldGenerationRoutine());
        }

        private IEnumerator ShieldGenerationRoutine()
        {
            yield return new WaitForSeconds(shieldGenDelay);
            CreateShield();
        }

        private void CreateShield()
        {
            //_hasShield = true;
            if (shieldPrefab != null) shieldPrefab.SetActive(true);
            
            ApplyAttackBuff(1.2f);
        }

        private void BreakShield()
        {
            //_hasShield = false;
            if (shieldPrefab != null) shieldPrefab.SetActive(false);
            
            ApplyAttackBuff(1.0f);
        }

        private void ApplyAttackBuff(float multiplier)
        {
            damage = _originDamage * multiplier;
        }
    }
}