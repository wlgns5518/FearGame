public class GroundedState : IState
{
    private PlayerMove player;
    private StateMachine sm;

    public GroundedState(PlayerMove player, StateMachine sm)
    {
        this.player = player;
        this.sm = sm;
    }

    public void Enter() { }

    public void Update()
    {
        player.ApplyGravity();
        player.Move();

        if (!player.controller.isGrounded)
        {
            sm.ChangeState(player.airState);
            return;
        }

        // 점프 입력 감지 (트리거)
        if (player.ConsumeJumpTriggered())
        {
            // Shift 같이 눌려있으면 차지
            if (player.sprinting)
            {
                sm.ChangeState(player.chargeState);
            }
            else
            {
                //그냥 점프
                player.Jump(1f);
                sm.ChangeState(player.airState);
            }
        }
    }

    public void Exit() { }
}