using UnityEngine;

public class EnemyHp : MonoBehaviour
{
    [SerializeField] private EnemyData enemyData;
    public float maxHp;
    public float currentHp;

    private void Start()
    {
        maxHp = enemyData.healthPoint;
        currentHp = maxHp;
    }

    public void TakeDamage(float damage)
    {
        currentHp -= damage;

        if (currentHp <= 0)
            Die();
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}
