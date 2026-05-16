using UnityEngine;

namespace _01.Scripts._06.Weapon.PlasmaGun
{
    public class PlasmaGunSkill : WeaponSkillBase
    {
        [SerializeField] private GameObject skillAttackPrefab;
        
        public override void Activate()
        {
            GameObject skillAttack = Instantiate(skillAttackPrefab, new Vector3(2f, 1f, 0f), Quaternion.identity);
            skillAttack.GetComponent<AttackObject>().Init(Owner.skillDamage);
            skillAttack.SetActive(true);
        }
    }
}
