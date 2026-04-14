using UnityEngine;

public abstract class WeaponSkillBase : MonoBehaviour
{
    protected WeaponController owner;

    public bool isSkilling;

    public virtual void Init(WeaponController owner)
    {
        this.owner = owner;
    }

    public abstract void Activate();
}
