using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuzakuRobotBossController : MonoBehaviour
{
    private enum PatternKind
    {
        Meteor,
        SummonMobs,
        Bullet,
        Rush
    }

    [Header("Components")]
    [SerializeField] private BossHp bossHp;

    [Header("Meteor Pattern")]
    [SerializeField] private float meteorPatternCooldown = 7f;
    [SerializeField] private GameObject meteorPrefab;
    [SerializeField] private float meteorMinX = -3f;
    [SerializeField] private float meteorMaxX = 10f;
    [SerializeField] private float meteorSpawnY = 6f;

    [Header("Summon Pattern")]
    [SerializeField] private float summonPatternCooldown = 10f;
    [SerializeField] private Transform summonPointA;
    [SerializeField] private Transform summonPointB;
    [SerializeField] private GameObject summonMobA;
    [SerializeField] private GameObject summonMobB;

    [Header("Bullet Pattern")]
    [SerializeField] private float bulletPatternCooldown = 10f;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private int bulletCount = 6;
    [SerializeField] private float bulletInterval = 1f;
    [SerializeField] private Transform bulletFirePointHead;
    [SerializeField] private Transform bulletFirePointBody;
    [SerializeField] private Transform bulletFirePointLeg;

    [Header("Shield Pattern")]
    [SerializeField] private float phase2HpRatio = 0.5f;
    [SerializeField] private float shieldDuration = 10f;
    [SerializeField] private float phase2StartDelay = 5f;
    [SerializeField] private GameObject shieldPrefab;
    [SerializeField] private Transform shieldPointA;
    [SerializeField] private Transform shieldPointB;
    [SerializeField] private GameObject breathPrefab;
    [SerializeField] private Transform breathFirePoint;

    [Header("Rush Pattern")]
    [SerializeField] private float rushPatternCooldown = 12f;
    [SerializeField] private Transform upperRushPoint;
    [SerializeField] private Transform lowerRushPoint;
    [SerializeField] private float moveToRushPointSpeed = 8f;
    [SerializeField] private float rushSpeed = 20f;
    [SerializeField] private float returnSpeed = 10f;
    [SerializeField] private float rushEndX = -15f;
    [SerializeField] private float returnStartXOffset = 10f;

    [Header("Pattern Option")]
    [SerializeField] private float delayBetweenPatterns = 1f;

    private List<PatternRuntimeData> runtimePatterns = new List<PatternRuntimeData>();

    private bool isPatternRunning;
    private bool isPhase2;
    private bool isPhaseChanging;

    private GameObject shieldA;
    private GameObject shieldB;

    private Vector3 originalPosition;

    private void Awake()
    {
        if (bossHp == null)
            bossHp = GetComponent<BossHp>();

        originalPosition = transform.position;

        InitPatterns();
    }

    private void Start()
    {
        StartCoroutine(PatternLoop());
    }

    private void Update()
    {
        UpdateCooldowns();
        CheckPhase2Condition();
    }

    private void InitPatterns()
    {
        runtimePatterns.Clear();

        runtimePatterns.Add(
            new PatternRuntimeData(PatternKind.Meteor, meteorPatternCooldown)
        );

        runtimePatterns.Add(
            new PatternRuntimeData(PatternKind.SummonMobs, summonPatternCooldown)
        );

        runtimePatterns.Add(
            new PatternRuntimeData(PatternKind.Bullet, bulletPatternCooldown)
        );

        runtimePatterns.Add(
            new PatternRuntimeData(PatternKind.Rush, rushPatternCooldown)
        );
    }

    private void UpdateCooldowns()
    {
        if (isPhaseChanging) return;

        foreach (PatternRuntimeData runtimePattern in runtimePatterns)
        {
            if (runtimePattern.currentCooldown > 0f)
            {
                runtimePattern.currentCooldown -= Time.deltaTime;
            }
        }
    }

    private void CheckPhase2Condition()
    {
        if (isPhase2 || isPhaseChanging) return;
        if (bossHp == null) return;

        float hpRatio = bossHp.currentHp / bossHp.maxHp;

        if (hpRatio <= phase2HpRatio)
        {
            StartCoroutine(Phase2TransitionRoutine());
        }
    }

    private IEnumerator PatternLoop()
    {
        while (true)
        {
            if (isPhaseChanging)
            {
                yield return null;
                continue;
            }

            if (isPatternRunning)
            {
                yield return null;
                continue;
            }

            List<PatternRuntimeData> readyPatterns = GetReadyPatterns();

            if (readyPatterns.Count == 0)
            {
                yield return null;
                continue;
            }

            PatternRuntimeData selectedPattern =
                readyPatterns[Random.Range(0, readyPatterns.Count)];

            yield return StartCoroutine(UsePattern(selectedPattern));

            yield return new WaitForSeconds(delayBetweenPatterns);
        }
    }

    private List<PatternRuntimeData> GetReadyPatterns()
    {
        List<PatternRuntimeData> readyPatterns = new List<PatternRuntimeData>();

        foreach (PatternRuntimeData runtimePattern in runtimePatterns)
        {
            if (runtimePattern.kind == PatternKind.Rush && !isPhase2)
                continue;

            if (runtimePattern.currentCooldown <= 0f)
            {
                readyPatterns.Add(runtimePattern);
            }
        }

        return readyPatterns;
    }

    private IEnumerator UsePattern(PatternRuntimeData runtimePattern)
    {
        isPatternRunning = true;

        switch (runtimePattern.kind)
        {
            case PatternKind.Meteor:
                SpawnMeteor();
                break;

            case PatternKind.SummonMobs:
                SpawnSummonMobs();
                break;

            case PatternKind.Bullet:
                yield return StartCoroutine(SpawnBulletPattern());
                break;

            case PatternKind.Rush:
                yield return StartCoroutine(RushPatternRoutine());
                break;
        }

        runtimePattern.currentCooldown = runtimePattern.cooldown;

        yield return null;

        isPatternRunning = false;
    }

    private IEnumerator Phase2TransitionRoutine()
    {
        isPhaseChanging = true;
        isPatternRunning = true;

        SpawnShields();

        yield return new WaitForSeconds(shieldDuration);

        bool bothShieldsDestroyed = shieldA == null && shieldB == null;

        DestroyRemainingShields();

        if (!bothShieldsDestroyed)
        {
            FireBreathOnce();
        }

        yield return new WaitForSeconds(phase2StartDelay);

        isPhase2 = true;
        isPhaseChanging = false;
        isPatternRunning = false;

        Debug.Log("2페이즈 시작");
    }

    private void SpawnShields()
    {
        if (shieldPrefab == null) return;

        if (shieldPointA != null)
        {
            shieldA = Instantiate(
                shieldPrefab,
                shieldPointA.position,
                Quaternion.identity
            );
        }

        if (shieldPointB != null)
        {
            shieldB = Instantiate(
                shieldPrefab,
                shieldPointB.position,
                Quaternion.identity
            );
        }
    }

    private void DestroyRemainingShields()
    {
        if (shieldA != null)
            Destroy(shieldA);

        if (shieldB != null)
            Destroy(shieldB);
    }

    private void FireBreathOnce()
    {
        if (breathPrefab == null || breathFirePoint == null)
            return;

        Instantiate(
            breathPrefab,
            breathFirePoint.position,
            Quaternion.identity
        );
    }

    private void SpawnMeteor()
    {
        if (meteorPrefab == null)
            return;

        float randomX = Random.Range(meteorMinX, meteorMaxX);

        Vector3 spawnPosition = new Vector3(
            randomX,
            meteorSpawnY,
            0f
        );

        Instantiate(
            meteorPrefab,
            spawnPosition,
            Quaternion.identity
        );
    }

    private void SpawnSummonMobs()
    {
        if (summonMobA != null && summonPointA != null)
        {
            Instantiate(
                summonMobA,
                summonPointA.position,
                Quaternion.identity
            );
        }

        if (summonMobB != null && summonPointB != null)
        {
            Instantiate(
                summonMobB,
                summonPointB.position,
                Quaternion.identity
            );
        }
    }

    private IEnumerator SpawnBulletPattern()
    {
        Transform[] bulletFirePoints =
        {
            bulletFirePointHead,
            bulletFirePointBody,
            bulletFirePointLeg
        };

        for (int i = 0; i < bulletCount; i++)
        {
            Transform randomFirePoint = bulletFirePoints[
                Random.Range(0, bulletFirePoints.Length)
            ];

            if (bulletPrefab != null && randomFirePoint != null)
            {
                Instantiate(
                    bulletPrefab,
                    randomFirePoint.position,
                    Quaternion.identity
                );
            }

            yield return new WaitForSeconds(bulletInterval);
        }
    }

    private IEnumerator RushPatternRoutine()
    {
        Transform selectedRushPoint = Random.value < 0.5f
            ? upperRushPoint
            : lowerRushPoint;

        if (selectedRushPoint == null)
            yield break;

        yield return StartCoroutine(MoveToPosition(
            selectedRushPoint.position,
            moveToRushPointSpeed
        ));

        Vector3 rushEndPosition = new Vector3(
            rushEndX,
            transform.position.y,
            transform.position.z
        );

        yield return StartCoroutine(MoveToPosition(
            rushEndPosition,
            rushSpeed
        ));

        Vector3 returnStartPosition = new Vector3(
            originalPosition.x + returnStartXOffset,
            originalPosition.y,
            originalPosition.z
        );

        transform.position = returnStartPosition;

        yield return StartCoroutine(MoveToPosition(
            originalPosition,
            returnSpeed
        ));
    }

    private IEnumerator MoveToPosition(Vector3 targetPosition, float speed)
    {
        while (Vector2.Distance(transform.position, targetPosition) > 0.05f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPosition,
                speed * Time.deltaTime
            );

            yield return null;
        }

        transform.position = targetPosition;
    }

    private class PatternRuntimeData
    {
        public PatternKind kind;
        public float cooldown;
        public float currentCooldown;

        public PatternRuntimeData(PatternKind kind, float cooldown)
        {
            this.kind = kind;
            this.cooldown = cooldown;
            currentCooldown = 0f;
        }
    }
}