// PlayerHFSM.cs
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerHFSM : HFSMRunner<PlayerContext>
{
    [SerializeField] private PlayerData playerData;

    public PlayerContext Context => context;
    public PlayerGroundedState groundedState { get; private set; }
    public PlayerAirState airState { get; private set; }


    protected override PlayerContext CreateContext()
    {
        return new PlayerContext
        {
            controller = GetComponent<CharacterController>(),
            transform = transform,
            data = playerData
        };
    }

    protected override IState CreateInitialState(PlayerContext context)
    {
        groundedState = new PlayerGroundedState(context, this);
        airState = new PlayerAirState(context, this);
        return groundedState;
    }
}