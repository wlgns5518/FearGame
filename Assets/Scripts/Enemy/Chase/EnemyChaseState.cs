public class EnemyChaseState : State<EnemyContext>
{
    private EnemyHFSM hfsm;
    public EnemyMoveState moveState;
    public EnemyAttackState attackState;
    public EnemyJumpState jumpState;
    public EnemyFallingState fallingState;

    public EnemyChaseState(EnemyContext context, EnemyHFSM hfsm) : base(context)
    {
        this.hfsm = hfsm;
        moveState = new EnemyMoveState(context, this);
        attackState = new EnemyAttackState(context, this);
        jumpState = new EnemyJumpState(context, this);
        fallingState = new EnemyFallingState(context, this);
        InitSubStateMachine(moveState);
    }

    public override void Enter() { base.Enter(); }

    public override void Update()
    {
        if (!context.PlayerInDetectionRange)
        {
            hfsm.ChangeRootState(hfsm.patrolState);
            return;
        }
        base.Update();
    }

    public override void Exit()
    {
        // Jump/Falling 중 루트 전환 시 agent 복구
        if (!context.agent.enabled)
        {
            context.agent.enabled = true;
            context.agent.Warp(context.transform.position);
        }
        base.Exit();
    }

    public void GoToMove() => ChangeSubState(moveState);
    public void GoToAttack() => ChangeSubState(attackState);
    public void GoToJump() => ChangeSubState(jumpState);
    public void GoToFalling() => ChangeSubState(fallingState);
}