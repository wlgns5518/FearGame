public class AirState : IState
{
    private PlayerMove player;
    private StateMachine sm;

    public AirState(PlayerMove player, StateMachine sm)
    {
        this.player = player;
        this.sm = sm;
    }

    public void Enter() { }

    public void Update()
    {
        player.ApplyGravity();
        player.Move();

        if (player.controller.isGrounded)
        {
            sm.ChangeState(player.groundedState);
        }
    }

    public void Exit() { }
}