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

        Debug.Log("드론 런처 스킬 발동!");
        StartCoroutine(SkillRoutine());
    }

    private IEnumerator SkillRoutine()
    {
        isSkilling = true;

        wc.weaponInfo.attackSpeed /= 1.5f;

        yield return new WaitForSeconds(duration);
        wc.weaponInfo.attackSpeed *= 1.5f;

        isSkilling = false;
    }
}