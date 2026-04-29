using UnityEngine;

public abstract class EnemyPatternBase : MonoBehaviour
{
    protected EnemyController owner;
    float damage;

    public virtual void Init(EnemyController owner, float damage)
    {
        this.owner = owner;
        this.damage = damage;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerHp>().TakeDamage(damage);
        }
    }
}
