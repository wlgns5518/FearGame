// EnemyFallingState.cs
public class EnemyFallingState : State<EnemyContext>
{
    private EnemyChaseState parent;

    public EnemyFallingState(EnemyContext context, EnemyChaseState parent) : base(context)
    {
        this.parent = parent;
    }

    public override void Enter() { }

    public override void Update()
    {
        context.ApplyGravityAndMove();

        if (context.controller.isGrounded)
        {
            context.agent.enabled = true;
            context.agent.Warp(context.transform.position);
            parent.GoToMove();
        }
    }

    public override void Exit() { }
}