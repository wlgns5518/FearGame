using UnityEngine;
using UnityEngine.AI;

public class EnemyWalkState : State<EnemyContext>
{
    private EnemyPatrolState parent;
    private float stuckTimer;
    private const float StuckThreshold = 2f;
    private Vector3 lastPosition;

    public EnemyWalkState(EnemyContext context, EnemyPatrolState parent) : base(context)
    {
        this.parent = parent;
    }

    public override void Enter()
    {
        stuckTimer = 0f;
        lastPosition = context.transform.position;
        context.animator?.Play("Walk");
        MoveToRandom();
    }

    public override void Update()
    {
        context.sound?.PlayFootstep();
        if (context.agent.pathPending) return;

        // 목적지 도착
        if (context.ReachedDestination())
        {
            parent.GoToWait();
            return;
        }

        // 경로 막힘 → 점프
        if (context.IsPathBlocked())
        {
            parent.GoToJump();
            return;
        }

        // stuck 감지
        float moved = Vector3.Distance(context.transform.position, lastPosition);
        if (moved < 0.05f)
        {
            stuckTimer += Time.deltaTime;
            if (stuckTimer >= StuckThreshold)
            {
                stuckTimer = 0f;
                // 장애물 있으면 점프, 없으면 새 위치로
                if (context.ObstacleAhead())
                    parent.GoToJump();
                else
                    MoveToRandom();
            }
        }
        else
        {
            stuckTimer = 0f;
            lastPosition = context.transform.position;
        }
    }

    public override void Exit() { }

    private void MoveToRandom()
    {
        Vector3 pos = NavMeshHelper.GetRandomWalkablePosition(context.transform.position);
        if (pos != Vector3.zero)
            context.MoveToPosition(pos, context.data.moveSpeed);
    }
}