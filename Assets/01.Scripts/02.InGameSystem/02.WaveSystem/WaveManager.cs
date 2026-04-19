using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private StageWaveData stageWaveData;
    [SerializeField] private SpawnPoint[] spawnPoints;

    private Dictionary<string, Transform> spawnPointMap = new Dictionary<string, Transform>();
    private List<ScheduledSpawn> scheduledSpawns = new List<ScheduledSpawn>();

    private float elapsedTime;
    private int currentSpawnIndex;
    private bool isPlaying;

    private void Awake()
    {
        foreach (SpawnPoint point in spawnPoints)
        {
            if (point == null) continue;

            if (!spawnPointMap.ContainsKey(point.pointId))
                spawnPointMap.Add(point.pointId, point.transform);
        }
    }

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
    }

    public void StartStage()
    {
        elapsedTime = 0f;
        currentSpawnIndex = 0;
        isPlaying = true;
    }

    private void BuildSchedule()
    {
        scheduledSpawns.Clear();

        foreach (WaveTimelineData timeline in stageWaveData.waveTimelines)
        {
            if (timeline == null) continue;

            foreach (SpawnEventData spawnEvent in timeline.spawnEvents)
            {
                if (spawnEvent == null || spawnEvent.prefab == null) continue;

                scheduledSpawns.Add(new ScheduledSpawn
                {
                    prefab = spawnEvent.prefab,
                    spawnPointId = spawnEvent.spawnPointId,
                    spawnTime = timeline.baseTime + spawnEvent.delayFromBaseTime
                });
            }
        }

        scheduledSpawns.Sort((a, b) => a.spawnTime.CompareTo(b.spawnTime));
    }

    private void Spawn(ScheduledSpawn scheduled)
    {
        if (!spawnPointMap.TryGetValue(scheduled.spawnPointId, out Transform point))
        {
            Debug.LogWarning($"SpawnPoint ID를 찾을 수 없음: {scheduled.spawnPointId}");
            return;
        }

        Instantiate(scheduled.prefab, point.position, Quaternion.identity);
    }

    private class ScheduledSpawn
    {
        public GameObject prefab;
        public string spawnPointId;
        public float spawnTime;
    }
}