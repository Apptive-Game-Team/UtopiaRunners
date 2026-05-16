using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

public class EdenMidBossLaserPattern : MonoBehaviour
{
    public float destroyTime = 0.3f;
    public float rotationZ = 31f;

    private void Start()
    {
        transform.rotation = Quaternion.Euler(0f, 0f, rotationZ);
    }

    private void Update()
    {
        StartCoroutine(DestroyLaser());
    }

    private IEnumerator DestroyLaser()
    {
        yield return new WaitForSeconds(destroyTime);
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
