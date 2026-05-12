using UnityEngine;

namespace _01.Scripts._06.Weapon.GravityCore
{
    public class GravityCoreSkill : WeaponSkillBase
    {
        [SerializeField] private GameObject skillAttackPrefab;
        
        public override void Activate()
        {
            Instantiate(skillAttackPrefab);
        }
    }
}
