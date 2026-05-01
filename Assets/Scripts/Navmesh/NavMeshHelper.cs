using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public static class NavMeshHelper
{
    // 캐싱
    private static List<int> validTriangles = new List<int>();
    private static NavMeshTriangulation triangulation;
    private static int walkableMask = -1;
    private static bool isInitialized = false;

    // 게임 시작 시 또는 씬 로드 후 1회 호출
    public static void Initialize()
    {
        isInitialized = false;
        validTriangles.Clear();

        int walkableIndex = NavMesh.GetAreaFromName("Walkable");
        if (walkableIndex < 0) { Debug.LogError("'Walkable' Area 없음"); return; }

        walkableMask = 1 << walkableIndex;
        triangulation = NavMesh.CalculateTriangulation(); // 1회만 실행

        if (triangulation.vertices.Length == 0) return;

        for (int i = 0; i < triangulation.indices.Length / 3; i++)
        {
            if ((1 << triangulation.areas[i] & walkableMask) != 0)
                validTriangles.Add(i);
        }

        isInitialized = validTriangles.Count > 0;
        if (!isInitialized) Debug.LogError("Walkable 삼각형 없음");
    }

    public static Vector3 GetRandomSpawnPosition()
    {
        if (!isInitialized) { Debug.LogError("NavMeshHelper.Initialize() 먼저 호출하세요"); return Vector3.zero; }

        for (int i = 0; i < 30; i++)
        {
            int triIndex = validTriangles[Random.Range(0, validTriangles.Count)];
            Vector3 v0 = triangulation.vertices[triangulation.indices[triIndex * 3]];
            Vector3 v1 = triangulation.vertices[triangulation.indices[triIndex * 3 + 1]];
            Vector3 v2 = triangulation.vertices[triangulation.indices[triIndex * 3 + 2]];

            float r1 = Mathf.Sqrt(Random.value);
            float r2 = Random.value;
            Vector3 randomPoint = (1 - r1) * v0 + (r1 * (1 - r2)) * v1 + (r1 * r2) * v2;

            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 2f, walkableMask))
                return hit.position;
        }
        return Vector3.zero;
    }

    public static Vector3 GetRandomWalkablePosition(Vector3 fromPosition, float radius = 15f)
    {
        if (!isInitialized) { Debug.LogError("NavMeshHelper.Initialize() 먼저 호출하세요"); return Vector3.zero; }

        for (int i = 0; i < 30; i++)
        {
            Vector2 rand = Random.insideUnitCircle * radius;
            Vector3 candidate = fromPosition + new Vector3(rand.x, 0f, rand.y);
            NavMeshHit hit;
            if (NavMesh.SamplePosition(candidate, out hit, 3f, walkableMask))
                return hit.position;
        }

        for (int i = 0; i < 30; i++)
        {
            Vector2 rand = Random.insideUnitCircle * (radius * 3f);
            Vector3 candidate = fromPosition + new Vector3(rand.x, 0f, rand.y);
            NavMeshHit hit;
            if (NavMesh.SamplePosition(candidate, out hit, 3f, walkableMask))
                return hit.position;
        }

        return Vector3.zero;
    }
}