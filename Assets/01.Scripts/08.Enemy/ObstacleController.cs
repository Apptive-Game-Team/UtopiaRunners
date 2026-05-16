using _01.Scripts._07.Character;
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
            PlayerController playerHp = collision.GetComponent<PlayerController>();

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
