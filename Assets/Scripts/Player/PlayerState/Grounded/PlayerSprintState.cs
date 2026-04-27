public class PlayerSprintState : State<PlayerContext>
{
    private PlayerGroundedState parent;

    public PlayerSprintState(PlayerContext context, PlayerGroundedState parent) : base(context)
    {
        this.parent = parent;
    }

    public override void Enter()
    {
        context.animator.Play("Sprint");
    }
    public override void Update()
    {
        if (!context.sprinting)
        {
            parent.GoToWalk();
            return;
        }

        if (context.moveInput.magnitude <= 0.01f)
        {
            parent.GoToIdle();
            return;
        }

        if (context.ConsumeJumpTriggered())
        {
            parent.GoToCharge(); 
        }
    }

    public override void Exit() { }
}