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
        context.sound = GetComponentInChildren<EnemySound>(); // Ãß°¡
    }

    protected override EnemyContext CreateContext()
    {
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