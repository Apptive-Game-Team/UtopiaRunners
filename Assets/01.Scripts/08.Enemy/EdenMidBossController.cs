using System.Collections;
using UnityEngine;

public class EdenMidBossController : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private BossData bossData;

    [Header("Fire Points")]
    [SerializeField] private Transform headFirePoint;
    [SerializeField] private Transform bodyFirePoint;
    [SerializeField] private Transform legFirePoint;

    [Header("Pattern Option")]
    [SerializeField] private int bulletCount = 3;
    [SerializeField] private float bulletInterval = 0.15f;
    [SerializeField] private float laserInterval = 0.5f;
    [SerializeField] private float laserXOffset = -7f;

    private bool isUsingSkillPattern;

    private Transform[] firePoints;

    private BossPatternData normalPattern;
    private BossPatternData skillPattern;

    private void Awake()
    {
        firePoints = new Transform[]
        {
            headFirePoint,
            bodyFirePoint,
            legFirePoint
        };

        normalPattern = bossData.patterns[0];
        skillPattern = bossData.patterns[1];
    }

    private void Start()
    {
        StartCoroutine(NormalPatternRoutine());
        StartCoroutine(SkillPatternRoutine());
    }

    private IEnumerator NormalPatternRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(normalPattern.cooldown);

            if (isUsingSkillPattern)
                continue;

            yield return StartCoroutine(FireNormalPatternRoutine());
        }
    }

    private IEnumerator SkillPatternRoutine()
    {
        while (true)
        {
            isUsingSkillPattern = true;

            SpawnPattern(skillPattern, headFirePoint);
            yield return new WaitForSeconds(laserInterval);

            SpawnPattern(skillPattern, bodyFirePoint);
            yield return new WaitForSeconds(laserInterval);

            SpawnPattern(skillPattern, legFirePoint);
            yield return new WaitForSeconds(laserInterval);

            isUsingSkillPattern = false;

            yield return new WaitForSeconds(skillPattern.cooldown);
        }
    }

    private IEnumerator FireNormalPatternRoutine()
    {
        Transform randomPoint = firePoints[Random.Range(0, firePoints.Length)];

        for (int i = 0; i < bulletCount; i++)
        {
            SpawnPattern(normalPattern, randomPoint);

            yield return new WaitForSeconds(bulletInterval);
        }
    }

    private void SpawnPattern(BossPatternData patternData, Transform firePoint)
    {
        Vector3 spawnPosition = firePoint.position;

        if (patternData.attackType == EnemyAttackType.Laser)
        {
            spawnPosition += new Vector3(laserXOffset, 0f, 0f);
        }

        GameObject pattern = Instantiate(
            patternData.patternPrefab,
            spawnPosition,
            Quaternion.identity
        );

        EnemyPatternBase patternScript = pattern.GetComponent<EnemyPatternBase>();

        if (patternScript != null)
        {
            patternScript.Init(
                null,
                patternData.attackDamage,
                patternData.attackType
            );
        }
    }
}