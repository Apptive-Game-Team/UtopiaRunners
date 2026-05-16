using System.Collections;
using UnityEngine;

namespace _01.Scripts._06.Weapon.DroneLauncher
{
    public class DroneLauncherSkill : WeaponSkillBase
    {
        [SerializeField] private float duration = 5f;

        public override void Activate()
        {
            if (isSkilling) return;

            Debug.Log("드론 런처 스킬 발동!");
            StartCoroutine(SkillRoutine());
        }

        private IEnumerator SkillRoutine()
        {
            isSkilling = true;

            Owner.weaponInfo.attackSpeed /= 1.5f;

            yield return new WaitForSeconds(duration);
            Owner.weaponInfo.attackSpeed *= 1.5f;

            isSkilling = false;
        }
    }
}