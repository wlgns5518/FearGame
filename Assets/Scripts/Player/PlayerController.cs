// PlayerController.cs
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerHFSM hfsm;
    private PlayerLook look;
    private PlayerUI ui;

    private void Awake()
    {
        hfsm = GetComponent<PlayerHFSM>();
        look = GetComponent<PlayerLook>();
        ui = GetComponent<PlayerUI>();
        GameManager.Instance.player = this;
    }
    private void Update()
    {
        ui.UpdateChargeUI(
            hfsm.Context.ChargeRatio,
            hfsm.Context.isCharging
        );
    }

    private void LateUpdate()
    {
        look.Look();
    }
}