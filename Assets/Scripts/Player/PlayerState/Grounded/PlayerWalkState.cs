// PlayerWalkState.cs
public class PlayerWalkState : State<PlayerContext>
{
    private PlayerGroundedState parent;

    public PlayerWalkState(PlayerContext context, PlayerGroundedState parent) : base(context)
    {
        this.parent = parent;
    }

    public override void Enter()
    {
        context.animator.Play("Walk");
    }

    public override void Update()
    {
        if (context.moveInput.magnitude <= 0.01f)
        {
            parent.GoToIdle();
            return;
        }

        if (context.sprinting)
        {
            parent.GoToSprint();
            return;
        }

        if (context.ConsumeJumpTriggered())
        {
            context.sound.PlayJump(); // 캐싱된 참조 사용
            context.Jump();
        }
    }

    public override void Exit() { }
}