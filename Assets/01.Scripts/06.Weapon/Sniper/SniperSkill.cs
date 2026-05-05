using System.Collections;
using UnityEngine;

namespace _01.Scripts._06.Weapon.Sniper
{
    public class SniperSkill : WeaponSkillBase
    {
        public override void Activate()
        {
            if (Owner is SniperController sc)
            {
                StartCoroutine(SkillRoutine(sc));
            }
        }

        private IEnumerator SkillRoutine(SniperController sc)
        {
            sc.isSkillUsed = true;
            yield return new WaitForSeconds(5f);
            sc.isSkillUsed = false;
        }
    }
}
