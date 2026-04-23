// EnemyPatrolState.cs
using UnityEngine;

public class EnemyPatrolState : State<EnemyContext>
{
    private EnemyHFSM hfsm;

    public EnemyWalkState walkState;
    public EnemyWaitState waitState;

    public EnemyPatrolState(EnemyContext context, EnemyHFSM hfsm) : base(context)
    {
        this.hfsm = hfsm;

        walkState = new EnemyWalkState(context, this);
        waitState = new EnemyWaitState(context, this);

        InitSubStateMachine(walkState);
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        // 상위 전환: 플레이어 감지
        if (context.PlayerInDetectionRange)
        {
            hfsm.ChangeRootState(hfsm.chaseState);
            return;
        }

        base.Update();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public void GoToWalk() => ChangeSubState(walkState);
    public void GoToWait() => ChangeSubState(waitState);
}