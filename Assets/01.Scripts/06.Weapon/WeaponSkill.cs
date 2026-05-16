using System.Collections;
using _01.Scripts._06.Weapon;
using UnityEngine;

/// <summary>
/// ���� ��ų ��ũ��Ʈ �⺻ ����
/// </summary>
public class WeaponSkill : WeaponSkillBase
{
    [SerializeField] private float duration = 1f;

    public override void Activate()
    {
        if (isSkilling) return;

        StartCoroutine(SkillRoutine());
    }

    private IEnumerator SkillRoutine()
    {
        isSkilling = true;

        yield return new WaitForSeconds(duration);

        isSkilling = false;
    }
}