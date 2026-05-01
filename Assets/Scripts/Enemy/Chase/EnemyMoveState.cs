using System.Collections;
using UnityEngine;
public class EnemyMoveState : State<EnemyContext>
{
    private EnemyChaseState parent;
    private float stuckTimer;
    private const float StuckThreshold = 1.5f;
    private const float StuckSqrDistance = 0.01f; // 0.1f * 0.1f
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

        if (context.IsPathBlocked() || context.ObstacleAhead())
        {
            parent.GoToJump();
            return;
        }

        float heightDiff = context.player.position.y - context.transform.position.y;

        // 플레이어가 위에 있을 때 → 점프
        if (heightDiff >= 3f)
        {
            parent.GoToJump();
            return;
        }

        // 플레이어가 아래에 있을 때 → 낙하 상태로 전환
        if (heightDiff <= -2f)
        {
            parent.GoToFalling();
            return;
        }

        // sqrMagnitude로 sqrt 연산 절약
        float sqrMoved = (context.transform.position - lastPosition).sqrMagnitude;
        if (sqrMoved < StuckSqrDistance)
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