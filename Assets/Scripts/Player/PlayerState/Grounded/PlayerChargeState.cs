// PlayerChargeState.cs
using UnityEngine;

public class PlayerChargeState : State<PlayerContext>
{
    private PlayerGroundedState parent;

    public PlayerChargeState(PlayerContext context, PlayerGroundedState parent) : base(context)
    {
        this.parent = parent;
    }

    public override void Enter()
    {
        context.chargeTimer = 0f;
        context.isCharging = true;
    }

    public override void Update()
    {
        context.chargeTimer = Mathf.Min(
            context.chargeTimer + Time.deltaTime,
            context.data.chargeTime
        );

        if (!context.JumpPressed)
        {
            float ratio = context.chargeTimer / context.data.chargeTime;
            float multiplier = ratio < 0.1f
                ? 1f
                : Mathf.Lerp(1f, context.data.superJumpMultiplier, ratio);

            context.Jump(multiplier);
            parent.GoToIdle();
        }
    }

    public override void Exit()
    {
        context.isCharging = false;
    }
}