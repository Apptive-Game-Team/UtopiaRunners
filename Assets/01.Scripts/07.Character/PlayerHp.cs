using _01.Scripts._07.Character;
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

    private void Update()
    {
        if (currentHp <= 0)
            Die();
    }

    public void TakeDamage(float damage)
    {
        /*if (!pc.isInvincible)
            currentHp -= damage;*/
    }

    public void Die()
    {
        pc.isDead = true;
        gameObject.SetActive(false);
    }
}
