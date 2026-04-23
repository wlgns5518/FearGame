// KeySpawner.cs
using System.Collections.Generic;
using UnityEngine;

public class KeySpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private GameObject[] keyPrefabs;
    [SerializeField] private float minDistanceFromPlayer = 5f;
    [SerializeField] private float minDistanceBetweenKeys = 5f;

    private List<Vector3> spawnedPositions = new List<Vector3>();

    private void Start()
    {
        SpawnKeys();
    }

    private void SpawnKeys()
    {
        Transform player = GameManager.Instance.player.transform;

        foreach (GameObject keyPrefab in keyPrefabs)
        {
            int maxAttempts = 30;
            int attempts = 0;

            while (attempts < maxAttempts)
            {
                attempts++;

                Vector3 spawnPosition = NavMeshHelper.GetRandomSpawnPosition();
                if (spawnPosition == Vector3.zero) continue;

                if (Vector3.Distance(spawnPosition, player.position) < minDistanceFromPlayer)
                    continue;

                if (IsTooCloseToOtherKeys(spawnPosition))
                    continue;

                spawnedPositions.Add(spawnPosition);
                Instantiate(keyPrefab, spawnPosition, Quaternion.identity);
                Debug.Log($"{keyPrefab.name} 생성 완료: {spawnPosition}");
                break;
            }
        }
    }

    private bool IsTooCloseToOtherKeys(Vector3 position)
    {
        foreach (Vector3 spawnedPosition in spawnedPositions)
        {
            if (Vector3.Distance(position, spawnedPosition) < minDistanceBetweenKeys)
                return true;
        }
        return false;
    }
}