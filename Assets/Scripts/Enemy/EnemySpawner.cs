// EnemySpawner.cs
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private int spawnCount = 5;
    [SerializeField] private float minDistanceFromPlayer = 10f;

    private void Start()
    {
        SpawnEnemies();
    }

    private void SpawnEnemies()
    {
        Transform player = GameManager.Instance.player.transform;

        int spawned = 0;
        int maxAttempts = spawnCount * 10;
        int attempts = 0;

        while (spawned < spawnCount && attempts < maxAttempts)
        {
            attempts++;

            Vector3 spawnPosition = NavMeshHelper.GetRandomSpawnPosition();
            if (spawnPosition == Vector3.zero) continue;

            if (Vector3.Distance(spawnPosition, player.position) < minDistanceFromPlayer)
                continue;

            Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
            spawned++;
        }

        Debug.Log($"Enemy Spawner: {spawned}개 생성 완료");
    }
}