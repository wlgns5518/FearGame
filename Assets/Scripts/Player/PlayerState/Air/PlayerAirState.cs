// ChargeState 力芭
public class PlayerAirState : State<PlayerContext>
{
    private PlayerHFSM hfsm;

    public PlayerRisingState risingState;
    public PlayerFallingState fallingState;
    // ChargeState 力芭

    public PlayerAirState(PlayerContext context, PlayerHFSM hfsm) : base(context)
    {
        this.hfsm = hfsm;

        risingState = new PlayerRisingState(context, this);
        fallingState = new PlayerFallingState(context, this);
        // ChargeState 力芭

        InitSubStateMachine(risingState);
    }

    public override void Enter()
    {
        if (context.verticalVelocity < 0f)
            ChangeSubState(fallingState);

        base.Enter();
    }

    public override void Update()
    {
        context.ApplyGravity();
        context.Move();

        if (context.controller.isGrounded)
        {
            hfsm.ChangeRootState(hfsm.groundedState);
            return;
        }

        base.Update();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public void GoToRising() => ChangeSubState(risingState);
    public void GoToFalling() => ChangeSubState(fallingState);
    // GoToCharge() 力芭
}