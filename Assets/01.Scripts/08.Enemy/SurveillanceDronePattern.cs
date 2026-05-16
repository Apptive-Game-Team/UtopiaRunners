using UnityEngine;

public class SurveillanceDronePattern : EnemyPatternBase
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
}
