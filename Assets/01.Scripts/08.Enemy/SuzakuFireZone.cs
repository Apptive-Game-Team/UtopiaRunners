using UnityEngine;

public class SuzakuFireZone : MonoBehaviour
{
    public float speed = 10f;
    public float destroyX = -15f;

    private void Update()
    {
        FireZoneMoving();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerHp playerHp = collision.GetComponent<PlayerHp>();

            if (playerHp != null)
                playerHp.TakeDamage(10f);
        }
    }

    private void FireZoneMoving()
    {
        transform.position += Vector3.left * speed * Time.deltaTime;

        if (transform.position.x <= destroyX)
            Destroy(gameObject);
    }
}
