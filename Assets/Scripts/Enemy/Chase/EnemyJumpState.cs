using UnityEngine;
public class EnemyJumpState : State<EnemyContext>
{
    private EnemyChaseState parent;
    private float fallTimer;

    public EnemyJumpState(EnemyContext context, EnemyChaseState parent) : base(context)
    {
        this.parent = parent;
    }

    public override void Enter()
    {
        fallTimer = 0f;
        context.agent.enabled = false;
        float heightDiff = context.player.position.y - context.transform.position.y;
        float targetHeight = Mathf.Max(heightDiff + 3f, context.data.jumpHeight);
        float multiplier = targetHeight / context.data.jumpHeight;
        context.Jump(multiplier);
    }

    public override void Update()
    {
        fallTimer += Time.deltaTime;
        context.ApplyGravityAndMove();

        if (fallTimer > 0.2f && context.verticalVelocity < 0f)
            parent.GoToFalling();
    }

    public override void Exit() { }
}