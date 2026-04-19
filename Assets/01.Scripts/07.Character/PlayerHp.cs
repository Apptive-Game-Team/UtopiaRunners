using UnityEngine;

public class PlayerHp : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private TempCharacterData characterData;
    private PlayerController pc;

    public float maxHp;
    public float currentHp;

    private void Awake()
    {
        pc = GetComponent<PlayerController>();
    }

    private void Start()
    {
        maxHp = characterData.healthPoint;
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
        pc.isDead = true;
    }
}
