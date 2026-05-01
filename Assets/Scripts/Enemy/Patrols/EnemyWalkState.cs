using UnityEngine;

public class EnemyWalkState : State<EnemyContext>
{
    private EnemyPatrolState parent;
    private float stuckTimer;
    private const float StuckThreshold = 2f;
    private const float StuckSqrDistance = 0.0025f; // 0.05f * 0.05f
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

        if (context.ReachedDestination())
        {
            parent.GoToWait();
            return;
        }

        if (context.IsPathBlocked())
        {
            parent.GoToJump();
            return;
        }

        // sqrMagnitude로 sqrt 연산 제거
        float sqrMoved = (context.transform.position - lastPosition).sqrMagnitude;
        if (sqrMoved < StuckSqrDistance)
        {
            stuckTimer += Time.deltaTime;
            if (stuckTimer >= StuckThreshold)
            {
                stuckTimer = 0f;
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