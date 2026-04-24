using UnityEngine;
public class EnemyAttackState : State<EnemyContext>
{
    private EnemyChaseState parent;
    private PlayerRespawn playerRespawn;

    public EnemyAttackState(EnemyContext context, EnemyChaseState parent) : base(context)
    {
        this.parent = parent;
    }

    public override void Enter()
    {
        context.attackTimer = 0f;
        context.ResetPath();
        playerRespawn = context.player.GetComponent<PlayerRespawn>();
    }

    public override void Update()
    {
        if (!context.PlayerInAttackRange)
        {
            parent.GoToMove();
            return;
        }

        context.attackTimer += Time.deltaTime;
        if (context.attackTimer >= context.data.attackCooldown)
        {
            Attack();
            context.attackTimer = 0f;
        }
    }

    public override void Exit() { }

    private void Attack()
    {
        playerRespawn?.Respawn();
        context.transform.GetComponent<EnemySound>()?.PlayAttack();
    }
}