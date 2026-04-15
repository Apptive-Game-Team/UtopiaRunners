using System.Collections;
using UnityEngine;

public class DroneLauncherSkill : WeaponSkillBase
{
    WeaponController wc;

    [SerializeField] private float duration = 5f;

    private void Start()
    {
        wc = GetComponentInParent<WeaponController>();
    }

    public override void Activate()
    {
        if (isSkilling) return;

        Debug.Log("스킬 사용!");
        StartCoroutine(SkillRoutine());
    }

    private IEnumerator SkillRoutine()
    {
        isSkilling = true;

        wc.weaponData.attackSpeed /= 1.5f;

        yield return new WaitForSeconds(duration);
        wc.weaponData.attackSpeed *= 1.5f;

        isSkilling = false;
    }
}