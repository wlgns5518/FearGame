// EnemyFallingState.cs
public class EnemyFallingState : State<EnemyContext>
{
    private EnemyChaseState parent;

    public EnemyFallingState(EnemyContext context, EnemyChaseState parent) : base(context)
    {
        this.parent = parent;
    }

    public override void Enter()
    {
        context.animator?.Play("Falling");
    }

    public override void Update()
    {
        context.ApplyGravityAndMove();

        if (context.controller.isGrounded && context.verticalVelocity <= 0f)
        {
            if (context.WarpToNavMesh())
                parent.GoToMove();
        }
    }

    public override void Exit() { }
}