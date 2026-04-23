using UnityEngine;

public class EnemyPatrolJumpState : State<EnemyContext>
{
    private EnemyPatrolState parent;
    private float fallTimer;

    public EnemyPatrolJumpState(EnemyContext context, EnemyPatrolState parent) : base(context)
    {
        this.parent = parent;
    }

    public override void Enter()
    {
        fallTimer = 0f;
        context.agent.enabled = false;
        context.Jump(1f);
    }

    public override void Update()
    {
        fallTimer += Time.deltaTime;
        context.ApplyGravityAndMove();

        if (fallTimer > 0.3f && context.controller.isGrounded && context.verticalVelocity <= 0f)
        {
            if (context.WarpToNavMesh())
                parent.GoToWalk();
        }
    }

    public override void Exit() { }
}