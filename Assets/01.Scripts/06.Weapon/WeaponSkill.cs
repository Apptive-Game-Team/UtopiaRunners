using System.Collections;
using UnityEngine;

/// <summary>
/// 무기 스킬 스크립트 기본 형태
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