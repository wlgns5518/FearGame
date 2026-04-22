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
            if (context.sprinting)
            {
                parent.GoToCharge(); // Shift 누른 상태면 차지
            }
            else
            {
                context.Jump();      // 일반 점프
            }
        }
    }

    public override void Exit() { }
}