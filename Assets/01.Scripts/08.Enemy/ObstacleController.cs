using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    public float speed = 10f;
    public float destroyX = -15f;

    private void Update()
    {
        ObstacleMoving();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerHp playerHp = collision.GetComponent<PlayerHp>();

            if (playerHp != null)
                playerHp.TakeDamage(9999f);
        }
    }

    private void ObstacleMoving()
    {
        transform.position += Vector3.left * speed * Time.deltaTime;

        if (transform.position.x <= destroyX)
            Destroy(gameObject);
    }
}
