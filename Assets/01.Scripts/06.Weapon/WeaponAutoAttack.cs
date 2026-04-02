using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponAutoAttack : MonoBehaviour
{
    public WeaponData weaponData;
    public GameObject projectile;

    private void Start()
    {
        StartCoroutine(AutoAttack());
    }

    IEnumerator AutoAttack()
    {
        while (true)
        {
            Instantiate(projectile);
            yield return new WaitForSeconds(weaponData.attackSpeed);
        }
    }
}
