public class PlayerGroundedState : State<PlayerContext>
{
    private PlayerHFSM hfsm;

    public PlayerIdleState idleState;
    public PlayerWalkState walkState;
    public PlayerSprintState sprintState;
    public PlayerChargeState chargeState; // 추가

    public PlayerGroundedState(PlayerContext context, PlayerHFSM hfsm) : base(context)
    {
        this.hfsm = hfsm;

        idleState = new PlayerIdleState(context, this);
        walkState = new PlayerWalkState(context, this);
        sprintState = new PlayerSprintState(context, this);
        chargeState = new PlayerChargeState(context, this); // 추가

        InitSubStateMachine(idleState);
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        context.ApplyGravity();
        context.Move();

        if (!context.controller.isGrounded)
        {
            hfsm.ChangeRootState(hfsm.airState);
            return;
        }

        base.Update();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public void GoToWalk() => ChangeSubState(walkState);
    public void GoToIdle() => ChangeSubState(idleState);
    public void GoToSprint() => ChangeSubState(sprintState);
    public void GoToCharge() => ChangeSubState(chargeState);
}