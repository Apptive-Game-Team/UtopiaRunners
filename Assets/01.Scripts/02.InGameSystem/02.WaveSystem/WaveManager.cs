using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [Header("Wave Data")]
    [SerializeField] private StageWaveData stageWaveData;

    [Header("Systems")]
    [SerializeField] private EnemySlotManager enemySlotManager;

    [Header("Clear UI")]
    [SerializeField] private GameObject clearPanel;
    [SerializeField] private TMP_Text earnedGoldText;
    [SerializeField] private TMP_Text earnedEveMemoryText;

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

        CheckStageClear();
    }

    public void StartStage()
    {
        elapsedTime = 0f;
        currentSpawnIndex = 0;

        isPlaying = true;
        isClearChecked = false;
        isGameOver = false;

        if (clearPanel != null)
        {
            clearPanel.SetActive(false);
        }

        if (GoldManager.Instance != null)
        {
            GoldManager.Instance.ResetStageGold();
        }

        if (EveMemoryManager.Instance != null)
        {
            EveMemoryManager.Instance.ResetStageRecord();
        }

        Time.timeScale = 1f;
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

        Time.timeScale = 0f;

        int earnedGold = 0;
        int earnedEveMemory = 0;

        if (GoldManager.Instance != null)
        {
            earnedGold = GoldManager.Instance.ApplyClearGold();
        }

        if (EveMemoryManager.Instance != null)
        {
            earnedEveMemory = EveMemoryManager.Instance.ApplyStageClearMemory();
        }

        ShowClearPanel(earnedGold, earnedEveMemory);

        Debug.Log("∞‘¿” ≈¨∏ÆæÓ");
    }

    private void ShowClearPanel(int earnedGold, int earnedEveMemory)
    {
        if (clearPanel != null)
        {
            clearPanel.SetActive(true);
        }

        if (earnedGoldText != null)
        {
            earnedGoldText.text = $"»πµÊ ∞ÒµÂ: {earnedGold}";
        }

        if (earnedEveMemoryText != null)
        {
            earnedEveMemoryText.text = $"»πµÊ ¿Ã∫Í¿« ±‚æÔ: {earnedEveMemory}";
        }

        Time.timeScale = 0f;
    }

    private class ScheduledSpawn
    {
        public GameObject prefab;
        public EnemyLane lane;
        public float spawnTime;
    }
}