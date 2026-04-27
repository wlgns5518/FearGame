using UnityEngine;
using UnityEngine.AI;

public class EnemyMoveState : State<EnemyContext>
{
    private EnemyChaseState parent;
    private float stuckTimer;
    private const float StuckThreshold = 1.5f;
    private Vector3 lastPosition;

    public EnemyMoveState(EnemyContext context, EnemyChaseState parent) : base(context)
    {
        this.parent = parent;
    }

    public override void Enter()
    {
        stuckTimer = 0f;
        lastPosition = context.transform.position;
        context.animator?.Play("Chase");
    }

    public override void Update()
    {
        context.sound?.PlayFootstep();
        context.MoveToPosition(context.player.position, context.data.chaseSpeed);

        if (context.PlayerInAttackRange)
        {
            parent.GoToAttack();
            return;
        }

        if (context.agent.pathPending) return;

        // 경로 막힘 or 장애물 → 점프
        if (context.IsPathBlocked() || context.ObstacleAhead())
        {
            parent.GoToJump();
            return;
        }

        // 높이 차이 → 점프
        float heightDiff = context.player.position.y - context.transform.position.y;
        if (heightDiff >= 3f)
        {
            parent.GoToJump();
            return;
        }

        // stuck → 점프
        float moved = Vector3.Distance(context.transform.position, lastPosition);
        if (moved < 0.1f)
        {
            stuckTimer += Time.deltaTime;
            if (stuckTimer >= StuckThreshold)
            {
                stuckTimer = 0f;
                parent.GoToJump();
            }
        }
        else
        {
            stuckTimer = 0f;
            lastPosition = context.transform.position;
        }
    }

    public override void Exit()
    {
        context.ResetPath();
    }
}