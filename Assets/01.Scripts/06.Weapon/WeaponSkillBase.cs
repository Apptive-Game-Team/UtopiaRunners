using _01.Scripts._06.Weapon;
using UnityEngine;

public abstract class WeaponSkillBase : MonoBehaviour
{
    protected WeaponController Owner;

    public bool isSkilling;

    public virtual void Init(WeaponController owner)
    {
        Owner = owner;
    }

    public abstract void Activate();
}
