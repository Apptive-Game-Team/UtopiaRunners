using System.Collections;
using UnityEngine;

public class WeaponSkill : WeaponSkillBase
{
    [SerializeField] private float duration = 1f;

    public override void Activate()
    {
        if (IsSkilling) return;

        StartCoroutine(SkillRoutine());
    }

    private IEnumerator SkillRoutine()
    {
        IsSkilling = true;

        yield return new WaitForSeconds(duration);

        IsSkilling = false;
    }
}