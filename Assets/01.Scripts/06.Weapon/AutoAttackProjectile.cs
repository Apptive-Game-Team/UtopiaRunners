using System.Collections;
using UnityEngine;

public class AutoAttackProjectile : MonoBehaviour
{
    public float speed = 10f;

    public GameObject targetEnemy;
    private float damage;

    public void Init(float damage)
    {
        this.damage = damage;
    }

    private void Update()
    {
        if (targetEnemy == null)
        {
            targetEnemy = FindNearestEnemy();

            if (targetEnemy == null)
            {
                Destroy(gameObject);
                return;
            }
        }

        transform.position = Vector2.MoveTowards(
            transform.position,
            targetEnemy.transform.position,
            speed * Time.deltaTime
        );
    }
    
    private GameObject FindNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        GameObject nearestEnemy = null;
        float nearestDistance = float.MaxValue;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);

            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestEnemy = enemy;
            }
        }

        return nearestEnemy;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<EnemyHp>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}