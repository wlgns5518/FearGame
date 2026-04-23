// EnemyPatrolState.cs
public class EnemyPatrolState : State<EnemyContext>
{
    private EnemyHFSM hfsm;

    public EnemyWalkState walkState;
    public EnemyWaitState waitState;
    public EnemyPatrolJumpState jumpState; // 추가

    public EnemyPatrolState(EnemyContext context, EnemyHFSM hfsm) : base(context)
    {
        this.hfsm = hfsm;

        walkState = new EnemyWalkState(context, this);
        waitState = new EnemyWaitState(context, this);
        jumpState = new EnemyPatrolJumpState(context, this); // 추가

        InitSubStateMachine(walkState);
    }

    public override void Enter() { base.Enter(); }

    public override void Update()
    {
        if (context.PlayerInDetectionRange)
        {
            hfsm.ChangeRootState(hfsm.chaseState);
            return;
        }

        base.Update();
    }

    public override void Exit() { base.Exit(); }

    public void GoToWalk() => ChangeSubState(walkState);
    public void GoToWait() => ChangeSubState(waitState);
    public void GoToJump() => ChangeSubState(jumpState); // 추가
}