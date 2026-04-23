using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public static class NavMeshHelper
{
    public static Vector3 GetRandomSpawnPosition()
    {
        int walkableIndex = NavMesh.GetAreaFromName("Walkable");
        if (walkableIndex < 0) { Debug.LogError("'Walkable' Area 없음"); return Vector3.zero; }
        int walkableMask = 1 << walkableIndex;

        NavMeshTriangulation triangulation = NavMesh.CalculateTriangulation();
        if (triangulation.vertices.Length == 0) return Vector3.zero;

        // Walkable 삼각형만 수집
        List<int> validTriangles = new List<int>();
        for (int i = 0; i < triangulation.indices.Length / 3; i++)
        {
            if ((1 << triangulation.areas[i] & walkableMask) != 0)
                validTriangles.Add(i);
        }

        if (validTriangles.Count == 0) { Debug.LogError("Walkable 삼각형 없음"); return Vector3.zero; }

        // 랜덤 삼각형에서 점 추출  맵 크기 상관없이 항상 NavMesh 위
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
        int walkableIndex = NavMesh.GetAreaFromName("Walkable");
        if (walkableIndex < 0) { Debug.LogError("'Walkable' Area 없음"); return Vector3.zero; }
        int walkableMask = 1 << walkableIndex;

        for (int i = 0; i < 30; i++)
        {
            Vector2 rand = Random.insideUnitCircle * radius;
            Vector3 candidate = fromPosition + new Vector3(rand.x, 0f, rand.y);

            NavMeshHit hit;
            if (NavMesh.SamplePosition(candidate, out hit, 3f, walkableMask))
                return hit.position;
        }

        // 근처에서 못찾으면 반경 늘려서 재시도
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