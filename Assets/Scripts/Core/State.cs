// State.cs
// 모든 상태의 베이스 클래스
// 하위 상태머신을 선택적으로 보유
public abstract class State<TContext> : IState
{
    protected TContext context;
    private StateMachine<TContext> subStateMachine;

    protected State(TContext context)
    {
        this.context = context;
    }

    // 하위 상태머신 초기화 (하위 상태가 있는 상태만 호출)
    protected void InitSubStateMachine(State<TContext> defaultState)
    {
        subStateMachine = new StateMachine<TContext>();
        subStateMachine.Initialize(defaultState);
    }

    public virtual void Enter()
    {
        subStateMachine?.CurrentState?.Enter();
    }

    public virtual void Update()
    {
        subStateMachine?.Update();
    }

    public virtual void Exit()
    {
        subStateMachine?.CurrentState?.Exit();
    }

    // 하위 상태 전환
    protected void ChangeSubState(State<TContext> newState)
    {
        subStateMachine?.ChangeState(newState);
    }

    // 현재 하위 상태 조회
    public IState CurrentSubState => subStateMachine?.CurrentState; // protected → public
}