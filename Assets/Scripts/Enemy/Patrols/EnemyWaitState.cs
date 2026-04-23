// EnemyWaitState.cs
using UnityEngine;

public class EnemyWaitState : State<EnemyContext>
{
    private EnemyPatrolState parent;

    public EnemyWaitState(EnemyContext context, EnemyPatrolState parent) : base(context)
    {
        this.parent = parent;
    }

    public override void Enter()
    {
        context.waitTimer = 0f;
        context.agent.ResetPath();
    }

    public override void Update()
    {
        context.waitTimer += Time.deltaTime;

        if (context.waitTimer >= context.data.waitTime)
        {
            parent.GoToWalk();
        }
    }

    public override void Exit() { }
}