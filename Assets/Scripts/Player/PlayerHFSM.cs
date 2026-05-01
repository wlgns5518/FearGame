using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerHFSM : HFSMRunner<PlayerContext>
{
    [SerializeField] private PlayerData playerData;
    [SerializeField] private GameObject lantern;
    [SerializeField] private PlayerUI playerUI;

    public PlayerContext Context => context;
    public PlayerGroundedState groundedState { get; private set; }
    public PlayerAirState airState { get; private set; }

    protected new void Awake()
    {
        base.Awake();
        context.animator = GetComponentInChildren<Animator>();
        context.sound = GetComponentInChildren<PlayerSound>(); // Ä³½Ì
        context.lantern = lantern;

        context.onPopUpOpen += playerUI.OnPopUpOpen;
        context.onPopUpClose += playerUI.OnPopUpClose;
        context.onPauseToggled += playerUI.OnPauseToggle;
    }

    private void OnDestroy()
    {
        context.onPopUpOpen -= playerUI.OnPopUpOpen;
        context.onPopUpClose -= playerUI.OnPopUpClose;
        context.onPauseToggled -= playerUI.OnPauseToggle;
    }

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