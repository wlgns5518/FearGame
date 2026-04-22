// AirState.GoToCharge() → GroundedState.GoToCharge()로 변경
public class PlayerSprintState : State<PlayerContext>
{
    private PlayerGroundedState parent;

    public PlayerSprintState(PlayerContext context, PlayerGroundedState parent) : base(context)
    {
        this.parent = parent;
    }

    public override void Enter() { }

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

        // 스프린트 중 점프 → ChargeState (땅에서)
        if (context.ConsumeJumpTriggered())
        {
            parent.GoToCharge(); // 변경
        }
    }

    public override void Exit() { }
}