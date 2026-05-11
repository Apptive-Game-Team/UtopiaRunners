using UnityEngine;

namespace _01.Scripts._07.Character
{
    public class MortController : PlayerController
    {
        [Header("Enhancement Settings")]
        [SerializeField] private float maxEnhanceTime = 10f;
        [SerializeField] private float maxDamageMultiplier = 1.5f;

        private float _enhancementTimer;
        private float _originDamage;

        protected override void Update()
        {
            base.Update();
            
            if (_enhancementTimer < maxEnhanceTime)
            {
                _enhancementTimer += Time.deltaTime;
                UpdateWeaponDamage();
            }
        }

        private void UpdateWeaponDamage()
        {
            float progress = Mathf.Clamp01(_enhancementTimer / maxEnhanceTime);
            float currentMultiplier = Mathf.Lerp(1f, maxDamageMultiplier, progress);
            
            damage = _originDamage * currentMultiplier;
        }

        private void ResetEnhancement()
        {
            _enhancementTimer = 0f;
            UpdateWeaponDamage();
        }

        private void OnEnable()
        {
            if (!IsSet)
            {
                return;
            }
            
            OnJumpDetected += ResetEnhancement;
            OnSlideDetected += ResetEnhancement;
        }

        private void OnDisable()
        {
            if (!IsSet)
            {
                return;
            }
            
            OnJumpDetected -= ResetEnhancement;
            OnSlideDetected -= ResetEnhancement;
        }
        
        public override void Init()
        {
            _originDamage = damage;
            if (gameObject.activeSelf)
            {
                OnJumpDetected += ResetEnhancement;
                OnSlideDetected += ResetEnhancement;
            }
            base.Init();
        }
    }
}