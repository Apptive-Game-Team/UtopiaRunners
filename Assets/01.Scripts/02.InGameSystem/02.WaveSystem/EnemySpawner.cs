//using System.Collections;
//using UnityEngine;

//public class EnemySpawner : MonoBehaviour
//{
//    [SerializeField] private Transform[] spawnPoints;

//    public void StartEnemyWave(EnemyWaveData wave)
//    {
//        StartCoroutine(SpawnEnemyWaveRoutine(wave));
//    }

//    private IEnumerator SpawnEnemyWaveRoutine(EnemyWaveData wave)
//    {
//        for (int i = 0; i < wave.spawnCount; i++)
//        {
//            Transform point = spawnPoints[Random.Range(0, spawnPoints.Length)];

//            Instantiate(wave.enemyPrefab, point.position, Quaternion.identity);

//            yield return new WaitForSeconds(wave.spawnInterval);
//        }
//    }
//}