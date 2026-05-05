using UnityEngine;

namespace _01.Scripts._06.Weapon
{
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
}
