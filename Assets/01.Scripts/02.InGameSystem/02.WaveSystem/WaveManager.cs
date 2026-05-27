using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [Header("Wave Data")]
    [SerializeField] private StageWaveData stageWaveData;

    [Header("Systems")]
    [SerializeField] private EnemySlotManager enemySlotManager;

    [Header("Game Over Option")]
    public float gameOverX = -8f;
    [SerializeField] private GameObject gameOverUI;

    private List<ScheduledSpawn> scheduledSpawns = new List<ScheduledSpawn>();

    private float elapsedTime;
    private int currentSpawnIndex;
    private bool isPlaying;
    private bool isClearChecked;
    private bool isGameOver;

    private void Start()
    {
        BuildSchedule();
        StartStage();
    }

    private void Update()
    {
        if (!isPlaying) return;

        elapsedTime += Time.deltaTime;

        while (currentSpawnIndex < scheduledSpawns.Count &&
               elapsedTime >= scheduledSpawns[currentSpawnIndex].spawnTime)
        {
            Spawn(scheduledSpawns[currentSpawnIndex]);
            currentSpawnIndex++;
        }

        CheckEnemyOverflow();
        CheckStageClear();
    }

    public void StartStage()
    {
        elapsedTime = 0f;
        currentSpawnIndex = 0;

        isPlaying = true;
        isClearChecked = false;
        isGameOver = false;

        if (GoldManager.Instance != null)
        {
            GoldManager.Instance.ResetStageGold();
        }

        if (EveMemoryManager.Instance != null)
        {
            EveMemoryManager.Instance.ResetStageRecord();
        }
    }

    private void BuildSchedule()
    {
        scheduledSpawns.Clear();

        if (stageWaveData == null) return;

        foreach (WaveTimelineData timeline in stageWaveData.waveTimelines)
        {
            if (timeline == null) continue;

            foreach (SpawnEventData spawnEvent in timeline.spawnEvents)
            {
                if (spawnEvent == null || spawnEvent.prefab == null) continue;

                scheduledSpawns.Add(new ScheduledSpawn
                {
                    prefab = spawnEvent.prefab,
                    lane = spawnEvent.lane,
                    spawnTime = timeline.baseTime + spawnEvent.delayFromBaseTime
                });
            }
        }

        scheduledSpawns.Sort((a, b) => a.spawnTime.CompareTo(b.spawnTime));
    }

    private void Spawn(ScheduledSpawn scheduled)
    {
        if (enemySlotManager == null) return;

        enemySlotManager.SpawnEnemy(
            scheduled.prefab,
            scheduled.lane
        );
    }

    private void CheckStageClear()
    {
        if (isClearChecked) return;
        if (isGameOver) return;

        bool allWavesSpawned = currentSpawnIndex >= scheduledSpawns.Count;

        if (!allWavesSpawned) return;

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        if (enemies.Length == 0)
        {
            StageClear();
        }
    }

    private void StageClear()
    {
        isClearChecked = true;
        isPlaying = false;

        if (GoldManager.Instance != null)
        {
            GoldManager.Instance.ApplyClearGold();
        }

        if (EveMemoryManager.Instance != null)
        {
            EveMemoryManager.Instance.ApplyStageClearMemory();
        }

        Debug.Log("게임 클리어");
    }

    private void CheckEnemyOverflow()
    {
        if (isGameOver) return;
        if (isClearChecked) return;

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in enemies)
        {
            if (enemy.transform.position.x <= gameOverX)
            {
                GameOver();
                return;
            }
        }
    }

    private void GameOver()
    {
        isGameOver = true;
        isPlaying = false;

        Debug.Log("게임 오버");

        if (gameOverUI != null)
            gameOverUI.SetActive(true);

        Time.timeScale = 0f;
    }

    private class ScheduledSpawn
    {
        public GameObject prefab;
        public EnemyLane lane;
        public float spawnTime;
    }
}