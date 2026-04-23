// NavMeshHelper.cs
using UnityEngine;
using UnityEngine.AI;

public static class NavMeshHelper
{
    public static Vector3 GetRandomSpawnPosition()
    {
        int walkableAreaMask = 1 << NavMesh.GetAreaFromName("Walkable");

        NavMeshTriangulation triangulation = NavMesh.CalculateTriangulation();

        if (triangulation.vertices.Length == 0)
        {
            Debug.LogWarning("NavMesh가 없습니다.");
            return Vector3.zero;
        }

        Bounds bounds = new Bounds(triangulation.vertices[0], Vector3.zero);
        foreach (Vector3 vertex in triangulation.vertices)
            bounds.Encapsulate(vertex);

        int maxAttempts = 100;

        for (int i = 0; i < maxAttempts; i++)
        {
            Vector3 randomPosition = new Vector3(
                Random.Range(bounds.min.x, bounds.max.x),
                bounds.center.y,
                Random.Range(bounds.min.z, bounds.max.z)
            );

            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPosition, out hit, 10f, walkableAreaMask))
                return hit.position;
        }

        Debug.LogWarning("Walkable Area에서 위치를 찾지 못했습니다.");
        return Vector3.zero;
    }
}