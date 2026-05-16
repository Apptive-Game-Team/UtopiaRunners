using _01.Scripts._07.Character;
using UnityEngine;

public abstract class EnemyPatternBase : MonoBehaviour
{
    protected EnemyController owner;
    float damage;
    EnemyAttackType attackType;

    public virtual void Init(EnemyController owner, float damage, EnemyAttackType attackType)
    {
        this.owner = owner;
        this.damage = damage;
        this.attackType = attackType;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerController>().TakeDamage(damage);

            if (attackType == EnemyAttackType.Bullet)
                Destroy(gameObject);
        }
    }
}
