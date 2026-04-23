// EnemyMoveState.cs
using UnityEngine;

public class EnemyMoveState : State<EnemyContext>
{
    private EnemyChaseState parent;

    private float stuckTimer;
    private float stuckThreshold = 1.5f; // 1.5초 동안 못움직이면 점프
    private Vector3 lastPosition;

    public EnemyMoveState(EnemyContext context, EnemyChaseState parent) : base(context)
    {
        this.parent = parent;
    }

    public override void Enter()
    {
        stuckTimer = 0f;
        lastPosition = context.transform.position;
    }

    public override void Update()
    {
        context.MoveToPosition(
            context.player.position,
            context.data.chaseSpeed
        );

        if (context.PlayerInAttackRange)
        {
            parent.GoToAttack();
            return;
        }

        float heightDiff = context.player.position.y - context.transform.position.y;

        // 장애물 감지
        if (context.ObstacleAhead())
        {
            parent.GoToJump();
            return;
        }

        // NavMesh 경로 끊김 감지
        if (context.IsPathBlocked())
        {
            parent.GoToJump();
            return;
        }

        // 일정 시간 동안 제자리면 점프
        float movedDistance = Vector3.Distance(context.transform.position, lastPosition);
        if (movedDistance < 0.1f)
        {
            stuckTimer += Time.deltaTime;
            if (stuckTimer >= stuckThreshold)
            {
                stuckTimer = 0f;
                parent.GoToJump();
                return;
            }
        }
        else
        {
            stuckTimer = 0f;
            lastPosition = context.transform.position;
        }

        // 높이 차이 3 이상이면 점프
        if (heightDiff >= 3f)
        {
            parent.GoToJump();
        }
    }

    public override void Exit()
    {
        context.agent.ResetPath();
    }
}