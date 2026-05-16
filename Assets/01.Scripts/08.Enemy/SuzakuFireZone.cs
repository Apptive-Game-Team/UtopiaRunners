using _01.Scripts._07.Character;
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
            PlayerController playerHp = collision.GetComponent<PlayerController>();

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
