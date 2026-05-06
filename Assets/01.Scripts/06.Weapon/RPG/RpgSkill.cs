using UnityEngine;

namespace _01.Scripts._06.Weapon.RPG
{
    public class RpgSkill : WeaponSkillBase
    {
        [SerializeField] private GameObject skillAttackPrefab;
        
        public override void Activate()
        {
            // Hp깎는 로직 추가
            GameObject skillAttack = Instantiate(skillAttackPrefab, new Vector3(2f, 1f, 0f), Quaternion.identity);
            skillAttack.GetComponent<AttackObject>().Init(Owner.skillDamage * 5f, true, 
                true, 1f, 1f, 5.1f);
            skillAttack.SetActive(true);
        }
    }
}
