using UnityEngine;

public class EdenCostablePattern : EnemyPatternBase
{
    float speed = 10f;
    float destroyX = -15f;
    Vector2 direction;

    private void Start()
    {
        direction = (GameObject.FindWithTag("Player").transform.position - transform.position).normalized;
    }

    private void Update()
    {
        BulletMoving();
    }

    private void BulletMoving()
    {
        transform.position += (Vector3)(direction * speed * Time.deltaTime);
        if (transform.position.x < destroyX)
        {
            Destroy(gameObject);
        }
    }
}
