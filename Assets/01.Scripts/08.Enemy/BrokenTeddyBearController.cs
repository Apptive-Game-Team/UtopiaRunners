using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace _01.Scripts._08.Enemy
{
    public class BrokenTeddyBearController : MonoBehaviour
    {
        private enum PatternKind
        {
            HipDrop,
            SummonRobots
        }

        [Header("Components")]
        [SerializeField] private BossHp bossHp;
        [SerializeField] private BossData data;

        [Header("Basic Attack (Button Throw)")]
        [SerializeField] private BrokenTeddyBearButton[] buttonPrefabs;

        [Header("Pattern - Hip Drop")]
        [SerializeField] private float hipDropCooldown = 8f;
        [SerializeField] private float jumpHeight = 5f;
        [SerializeField] private float jumpDuration = 0.6f;
        [SerializeField] private float landDuration = 0.2f;
        [SerializeField] private GameObject groundShockwavePrefab; // 전체 바닥 대미지 이펙트/콜라이더

        [Header("Pattern - Summon & Combine Robots")]
        [SerializeField] private float summonCooldown = 12f;
        [SerializeField] private GameObject minionRobotPrefab;
        [SerializeField] private Transform[] robotSpawnPoints;
        [SerializeField] private GameObject combinedRobotPrefab; // 4마리 합체 시 스폰될 거대 로봇
        [SerializeField] private float combinedSpawnMinY = -3f;
        [SerializeField] private float combinedSpawnMaxY = 3f;

        [Header("Pattern Option")]
        [SerializeField] private float delayBetweenPatterns = 1.5f;
        
        private List<GameObject> _activeMinions = new();
        private List<PatternRuntimeData> _runtimePatterns = new();
        
        private bool _isPatternRunning;
        private Vector3 _originalPosition;

        private void Awake()
        {
            if (bossHp == null)
                bossHp = GetComponent<BossHp>();

            _originalPosition = transform.position;
            InitPatterns();
        }

        private void Start()
        {
            StartCoroutine(BasicButtonAttackRoutine());
            StartCoroutine(PatternLoop());
        }

        private void Update()
        {
            UpdateCooldowns();
            CleanUpMinionList();
        }

        private void InitPatterns()
        {
            _runtimePatterns.Clear();

            _runtimePatterns.Add(new PatternRuntimeData(PatternKind.HipDrop, hipDropCooldown));
            _runtimePatterns.Add(new PatternRuntimeData(PatternKind.SummonRobots, summonCooldown));
        }

        private void UpdateCooldowns()
        {
            foreach (var pattern in _runtimePatterns)
            {
                if (pattern.currentCooldown > 0f)
                {
                    pattern.currentCooldown -= Time.deltaTime;
                }
            }
        }
        
        private void CleanUpMinionList()
        {
            _activeMinions.RemoveAll(robot => !robot);
        }
        
        private IEnumerator PatternLoop()
        {
            while (true)
            {
                if (_isPatternRunning)
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

                PatternRuntimeData selectedPattern = readyPatterns[Random.Range(0, readyPatterns.Count)];
                yield return StartCoroutine(UsePattern(selectedPattern));

                yield return new WaitForSeconds(delayBetweenPatterns);
            }
        }

        private List<PatternRuntimeData> GetReadyPatterns()
        {
            List<PatternRuntimeData> readyPatterns = new List<PatternRuntimeData>();

            foreach (var pattern in _runtimePatterns)
            {
                if (pattern.currentCooldown <= 0f)
                {
                    readyPatterns.Add(pattern);
                }
            }

            return readyPatterns;
        }

        private IEnumerator UsePattern(PatternRuntimeData pattern)
        {
            _isPatternRunning = true;

            switch (pattern.kind)
            {
                case PatternKind.HipDrop:
                    yield return StartCoroutine(HipDropPatternRoutine());
                    break;

                case PatternKind.SummonRobots:
                    yield return StartCoroutine(SummonOrCombineRoutine());
                    break;
            }

            pattern.currentCooldown = pattern.cooldown;
            yield return null;
            _isPatternRunning = false;
        }

        private IEnumerator BasicButtonAttackRoutine()
        {
            BossPatternData pd = data.patterns.Find(p => p.patternName == "Button");
            
            while (true)
            {
                yield return new WaitForSeconds(pd.cooldown);
                
                if (buttonPrefabs == null || buttonPrefabs.Length == 0)
                {
                    continue;
                }

                if (_isPatternRunning)
                {
                    continue;
                }
                
                BrokenTeddyBearButton selectedButton = buttonPrefabs[Random.Range(0, buttonPrefabs.Length)];
                selectedButton.InitSetting(pd.attackDamage);
                
                Vector3 targetPoint = selectedButton.targetPoint;
                
                GameObject buttonObj = Instantiate(selectedButton.gameObject, transform.position, Quaternion.identity);
                buttonObj.transform.DOJump(targetPoint, Vector3.Distance(transform.position, targetPoint), 1, 1f)
                    .SetEase(Ease.OutCubic)
                    .OnComplete(() =>
                    {
                        if (buttonObj)
                        {
                            buttonObj.GetComponent<BrokenTeddyBearButton>().Explode();
                        }
                    });
            }
        }
        
        private IEnumerator HipDropPatternRoutine()
        {
            _isPatternRunning = true;
            
            Vector3 peakPos = _originalPosition + Vector3.up * jumpHeight;

            Sequence seq = DOTween.Sequence();

            seq.Append(transform.DOMove(peakPos, jumpDuration).SetEase(Ease.OutCubic));
            seq.Append(transform.DOMove(_originalPosition, landDuration).SetEase(Ease.InCubic));

            yield return seq.WaitForCompletion();

            transform.position = _originalPosition;
            
            if (groundShockwavePrefab)
            {
                Instantiate(groundShockwavePrefab, transform.position, Quaternion.identity);
            }

            yield return new WaitForSeconds(0.5f);

            _isPatternRunning = false;
        }
        
        private IEnumerator SummonOrCombineRoutine()
        {
            _isPatternRunning = true;
            
            CleanUpMinionList();

            // 필드에 살아있는 로봇이 4마리 이상인 경우 -> 합체 로봇으로 변환!
            if (_activeMinions.Count >= 4)
            {
                // 1. 기존 잔여 로봇 4마리 파괴
                for (int i = _activeMinions.Count - 1; i >= 0; i--)
                {
                    if (_activeMinions[i] != null)
                    {
                        Destroy(_activeMinions[i]);
                    }
                }
                _activeMinions.Clear();

                // 2. 무작위 Y값 위치 계산 후 합체 로봇 생성
                float randomY = Random.Range(combinedSpawnMinY, combinedSpawnMaxY);
                Vector3 spawnPos = new Vector3(transform.position.x - 3f, randomY, transform.position.z);

                if (combinedRobotPrefab != null)
                {
                    Instantiate(combinedRobotPrefab, spawnPos, Quaternion.identity);
                }
            }
            else // 4마리 미만인 경우 -> 미니언 소환
            {
                if (robotSpawnPoints != null && minionRobotPrefab != null)
                {
                    foreach (Transform spawnPt in robotSpawnPoints)
                    {
                        GameObject minion = Instantiate(minionRobotPrefab, spawnPt.position, Quaternion.identity);
                        _activeMinions.Add(minion);
                    }
                }
            }

            yield return new WaitForSeconds(0.5f);

            _isPatternRunning = false;
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
}