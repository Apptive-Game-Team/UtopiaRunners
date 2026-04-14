using System.Collections;
using UnityEngine;

public class AutoAttackProjectile : MonoBehaviour
{
    public float speed = 10f;

    private Transform target;
    private float damage;

    //private void Start()
    //{
    //    StartCoroutine(ProjectileDestroy());
    //}

    public void Init(Transform target, float damage)
    {
        this.target = target;
        this.damage = damage;
    }

    private void Update()
    {
        transform.position = Vector2.MoveTowards(
            transform.position,
            target.position,
            speed * Time.deltaTime
        );
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            //collision.GetComponent<EnemyHealth>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }

    //private IEnumerator ProjectileDestroy()
    //{
    //    if (target == null)
    //    {
    //        yield return new WaitForSeconds(0.1f);
    //        if (target == null)
    //            Destroy(gameObject);
    //    }
    //}
}