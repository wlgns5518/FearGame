using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerHFSM hfsm;
    private PlayerLook look;
    public PlayerUI ui;

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

    public void OnKeyPickup()
    {
        hfsm.Context.AddKey();
        ui.UpdateKeyUI(hfsm.Context.KeyCount,hfsm.Context.KeyRequired);
        if (hfsm.Context.HasEnoughKeys())
            look.StartCinematic();
    }

    public void UseKeys()
    {
        hfsm.Context.UseKeys();
    }
    public bool HasEnoughKeys()
    {
        return hfsm.Context.HasEnoughKeys();
    }
}