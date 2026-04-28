using UnityEngine;

public abstract class EnemyPatternBase : MonoBehaviour
{
    protected EnemyController owner;

    public bool isSkilling;

    public virtual void Init(EnemyController owner, float damage)
    {
        this.owner = owner;
        //this.damage = damage;
    }

    public abstract void Activate();
}
