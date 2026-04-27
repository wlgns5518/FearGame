using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class EnemyHFSM : HFSMRunner<EnemyContext>
{
    [SerializeField] private EnemyData enemyData;

    public EnemyContext Context => context;
    public EnemyPatrolState patrolState { get; private set; }
    public EnemyChaseState chaseState { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        context.animator = GetComponentInChildren<Animator>();
    }

    protected override EnemyContext CreateContext()
    {
        // GameManager에서 플레이어 가져오기
        Transform player = GameManager.Instance.player.transform;

        return new EnemyContext
        {
            agent = GetComponent<UnityEngine.AI.NavMeshAgent>(),
            controller = GetComponent<CharacterController>(),
            transform = transform,
            player = player,
            data = enemyData
        };
    }

    protected override IState CreateInitialState(EnemyContext context)
    {
        patrolState = new EnemyPatrolState(context, this);
        chaseState = new EnemyChaseState(context, this);
        return patrolState;
    }
}