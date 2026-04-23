// EnemyWalkState.cs
using UnityEngine;
using UnityEngine.AI;

public class EnemyWalkState : State<EnemyContext>
{
    private EnemyPatrolState parent;

    public EnemyWalkState(EnemyContext context, EnemyPatrolState parent) : base(context)
    {
        this.parent = parent;
    }

    public override void Enter()
    {
        MoveToRandomPosition();
    }

    public override void Update()
    {
        if (context.ReachedDestination())
        {
            parent.GoToWait();
        }
    }

    public override void Exit() { }

    private void MoveToRandomPosition()
    {
        Vector3 randomPosition = GetRandomNavMeshPosition();
        context.MoveToPosition(randomPosition, context.data.moveSpeed);
    }

    private Vector3 GetRandomNavMeshPosition()
    {
        // 현재 위치 기준 랜덤 방향으로 순찰 반경 내 위치 탐색
        Vector3 randomDirection = Random.insideUnitSphere * context.data.patrolRadius;
        randomDirection += context.transform.position;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, context.data.patrolRadius, NavMesh.AllAreas))
            return hit.position;

        // 실패 시 현재 위치 반환
        return context.transform.position;
    }
}