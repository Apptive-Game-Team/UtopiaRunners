using UnityEngine;

public class EnemyHp : MonoBehaviour
{
    [SerializeField] private EnemyData enemyData;

    public float maxHp;
    public float currentHp;

    private bool isDead;

    private void Start()
    {
        maxHp = enemyData.healthPoint;
        currentHp = maxHp;
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;

        currentHp -= damage;

        if (currentHp <= 0)
            Die();
    }

    public void Die()
    {
        if (isDead) return;

        isDead = true;

        if (CompareTag("Enemy"))
        {
            if (GoldManager.Instance != null)
            {
                GoldManager.Instance.AddEnemyKillGold();
            }
        }

        Destroy(gameObject);
    }
}