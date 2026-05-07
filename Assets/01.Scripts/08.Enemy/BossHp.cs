using UnityEngine;

public class BossHp : MonoBehaviour
{
    [SerializeField] private BossData bossData;
    public float maxHp;
    public float currentHp;

    private void Start()
    {
        maxHp = bossData.healthPoint;
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
