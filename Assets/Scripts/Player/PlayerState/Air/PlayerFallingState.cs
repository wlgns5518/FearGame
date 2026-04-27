// PlayerFallingState.cs
public class PlayerFallingState : State<PlayerContext>
{
    private PlayerAirState parent;

    public PlayerFallingState(PlayerContext context, PlayerAirState parent) : base(context)
    {
        this.parent = parent;
    }

    public override void Enter()
    {
        context.animator.Play("Falling");
    }
    public override void Update()
    {
        if (context.verticalVelocity >= 0f)
            parent.GoToRising();
    }

    public override void Exit() { }
}