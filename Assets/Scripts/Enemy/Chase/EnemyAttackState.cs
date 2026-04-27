using System;
using System.Collections;
using UnityEngine;
public class EnemyAttackState : State<EnemyContext>
{
    private EnemyChaseState parent;
    private PlayerRespawn playerRespawn;
    private PlayerController playerController;

    public EnemyAttackState(EnemyContext context, EnemyChaseState parent) : base(context)
    {
        this.parent = parent;
    }

    public override void Enter()
    {
        context.attackTimer = 0f;
        context.ResetPath();
        context.animator?.Play("Attack");
        playerRespawn = context.player.GetComponent<PlayerRespawn>();
        playerController = context.player.GetComponent<PlayerController>();
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
        context.transform.GetComponent<EnemySound>()?.PlayAttack();
        playerRespawn?.Respawn();
        playerController.TakeDamage();
    }
}