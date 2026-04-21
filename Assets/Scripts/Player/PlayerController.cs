using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PlayerMove move;
    public PlayerLook look;
    public PlayerUI UI;

    private void Awake()
    {
        move = GetComponent<PlayerMove>();
        look = GetComponent<PlayerLook>();
        UI = GetComponent<PlayerUI>();
    }

    private void Update()
    {
        move.UpdateState();

        UI.UpdateChargeUI(move.ChargeRatio, move.IsCharging);
    }

    private void LateUpdate()
    {
        look.Look();
    }
}