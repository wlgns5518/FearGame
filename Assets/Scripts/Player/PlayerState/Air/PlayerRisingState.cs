// PlayerRisingState.cs
public class PlayerRisingState : State<PlayerContext>
{
    private PlayerAirState parent;

    public PlayerRisingState(PlayerContext context, PlayerAirState parent) : base(context)
    {
        this.parent = parent;
    }

    public override void Enter()
    {
        context.animator.Play("Jump");
    }
    public override void Update()
    {
        if (context.verticalVelocity < 0f)
            parent.GoToFalling();
    }

    public override void Exit() { }
}