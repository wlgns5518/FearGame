// EnemyJumpState.cs
using UnityEngine;

public class EnemyJumpState : State<EnemyContext>
{
    private EnemyChaseState parent;

    public EnemyJumpState(EnemyContext context, EnemyChaseState parent) : base(context)
    {
        this.parent = parent;
    }

    public override void Enter()
    {
        context.agent.enabled = false;

        float heightDiff = context.player.position.y - context.transform.position.y;
        float targetHeight = Mathf.Max(heightDiff + 3f, context.data.jumpHeight);
        float multiplier = targetHeight / context.data.jumpHeight;

        context.Jump(multiplier);
    }

    public override void Update()
    {
        context.ApplyGravityAndMove();

        // 하강 시작하면 FallingState로
        if (context.verticalVelocity < 0f)
        {
            parent.GoToFalling();
        }
    }

    public override void Exit() { }
}