using _01.Scripts._06.Weapon;
using UnityEngine;

namespace _01.Scripts._07.Character
{
    public class IcarusController : PlayerController
    {
        private WeaponController _wc;
        
        private void OnEnable()
        {
            if (!IsSet)
            {
                return;
            }
            
            OnJumpDetected += CoolDownSkill;
            OnSlideDetected += CoolDownSkill;
        }

        private void OnDisable()
        {
            if (!IsSet)
            {
                return;
            }
            
            OnJumpDetected -= CoolDownSkill;
            OnSlideDetected -= CoolDownSkill;
        }

        private void CoolDownSkill()
        {
            _wc.ReduceCooldown(10f);
        }

        public override void Init()
        {
            _wc = FindAnyObjectByType<WeaponController>();
            if (gameObject.activeSelf)
            {
                OnJumpDetected += CoolDownSkill;
                OnSlideDetected += CoolDownSkill;
            }
            base.Init();
        }
    }
}
