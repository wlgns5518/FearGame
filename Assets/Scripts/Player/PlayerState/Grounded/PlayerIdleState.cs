// PlayerIdleState.cs
public class PlayerIdleState : State<PlayerContext>
{
    private PlayerGroundedState parent;

    public PlayerIdleState(PlayerContext context, PlayerGroundedState parent) : base(context)
    {
        this.parent = parent;
    }

    public override void Enter() { }

    public override void Update()
    {
        if (context.moveInput.magnitude > 0.01f)
        {
            parent.GoToWalk();
            return;
        }

        if (context.ConsumeJumpTriggered())
        {
            context.transform.GetComponent<PlayerSound>().PlayJump(); // Ãß°¡
            if (context.sprinting)
                parent.GoToCharge();
            else
                context.Jump();
        }
    }

    public override void Exit() { }
}