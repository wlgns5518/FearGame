// PlayerIdleState.cs
public class PlayerIdleState : State<PlayerContext>
{
    private PlayerGroundedState parent;

    public PlayerIdleState(PlayerContext context, PlayerGroundedState parent) : base(context)
    {
        this.parent = parent;
    }

    public override void Enter()
    {
        context.animator.Play("Idle");
    }

    public override void Update()
    {
        if (context.moveInput.magnitude > 0.01f)
        {
            parent.GoToWalk();
            return;
        }

        if (context.ConsumeJumpTriggered())
        {
            context.sound.PlayJump(); // 캐싱된 참조 사용, 중복 제거
            if (context.sprinting)
                parent.GoToCharge();
            else
                context.Jump();
        }
    }

    public override void Exit() { }
}