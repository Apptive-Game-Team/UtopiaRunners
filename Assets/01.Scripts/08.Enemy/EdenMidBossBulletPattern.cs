using UnityEngine;

public class EdenMidBossBulletPattern : MonoBehaviour
{
    public float speed = 10f;
    public float destroyX = -15f;

    private void Update()
    {
        BulletMoving();
    }

    private void BulletMoving()
    {
        transform.position += Vector3.left * speed * Time.deltaTime;

        if (transform.position.x <= destroyX)
            Destroy(gameObject);
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.CompareTag("Player"))
    //    {
    //        collision.GetComponent<PlayerHp>().TakeDamage(damage);

    //        if (attackType == EnemyAttackType.Bullet)
    //            Destroy(gameObject);
    //    }
    //}
}
