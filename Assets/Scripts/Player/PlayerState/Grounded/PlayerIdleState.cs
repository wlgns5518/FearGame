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
            context.transform.GetComponent<PlayerSound>().PlayJump(); // 추가
            if (context.sprinting)
            {
                context.transform.GetComponent<PlayerSound>().PlayJump(); // 추가
                parent.GoToCharge();
            }
            else
            {
                context.transform.GetComponent<PlayerSound>().PlayJump();
                context.Jump();
            }
        }
    }

    public override void Exit() { }
}