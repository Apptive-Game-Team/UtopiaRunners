using UnityEngine;

namespace _01.Scripts._06.Weapon.MissilePod
{
    public class MissilePodSkill : WeaponSkillBase
    {
        [SerializeField] private GameObject missilePrefab;
        
        public override void Activate()
        {
            if (Owner is MissilePodController mc)
            {
                for (int i = 0; i < mc.missileCount; i++)
                {
                    Instantiate(missilePrefab, transform.position + (Vector3)Random.insideUnitCircle, Quaternion.identity);
                }

                mc.missileCount = 0;
            }
        }
    }
}
