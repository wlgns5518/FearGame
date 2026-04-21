using UnityEngine;

public class ChargeState : IState
{
    private PlayerMove player;
    private StateMachine sm;

    public ChargeState(PlayerMove player, StateMachine sm)
    {
        this.player = player;
        this.sm = sm;
    }

    public void Enter()
    {
        player.chargeTimer = 0f;
        player.isCharging = true;
    }

    public void Update()
    {
        player.ApplyGravity();
        player.Move();

        // ÃæÀü
         player.chargeTimer = Mathf.Min(
            player.chargeTimer + Time.deltaTime,
            player.chargeTime
        );

        if (!player.JumpPressed)
        {
            float ratio = player.chargeTimer / player.chargeTime;

            float multiplier;
            if (ratio < 0.1f)
                multiplier = 1f;
            else
                multiplier = Mathf.Lerp(1f, player.superJumpMultiplier, ratio);

            player.Jump(multiplier);
            sm.ChangeState(player.airState);
        }
    }

    public void Exit()
    {
        player.isCharging = false;
    }
}