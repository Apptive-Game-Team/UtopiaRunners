using UnityEngine;

public class SecurityRobotPattern : EnemyPatternBase
{
    public float speed = 10;
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
}
