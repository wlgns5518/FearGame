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
        // Enter에서 한 번만 캐싱
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
        context.sound?.PlayAttack(); // GetComponent 제거, 캐싱된 sound 사용
        playerRespawn?.Respawn();
        playerController?.TakeDamage();
    }
}