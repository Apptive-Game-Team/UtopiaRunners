using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Components")]
    public EnemyData enemyData;

    public GameObject firePoint;

    private void Start()
    {
        StartCoroutine(EnemyPattern());
    }

    private IEnumerator EnemyPattern()
    {
        while (true)
        {
            GameObject pattern = Instantiate(
                enemyData.enemyPatternPrefab,
                firePoint.transform.position,
                Quaternion.identity
            );

            EnemyPatternBase patternScript = pattern.GetComponent<EnemyPatternBase>();
            if (patternScript != null)
                patternScript.Init(this, enemyData.attackDamage, enemyData.attackType);

            yield return new WaitForSeconds(enemyData.cooldown);
        }
    }
}
