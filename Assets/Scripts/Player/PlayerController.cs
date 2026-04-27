using UnityEngine;
public class PlayerController : MonoBehaviour
{
    public PlayerHFSM hfsm;
    private PlayerLook look;
    public PlayerUI ui;

    [SerializeField] private EnemyData enemyData;

    private const float defaultDetectionRange = 20f;
    private const float lanternDetectionRange = 50f;

    private void Awake()
    {
        hfsm = GetComponent<PlayerHFSM>();
        look = GetComponent<PlayerLook>();
        ui = GetComponent<PlayerUI>();
        GameManager.Instance.player = this;
    }

    private void Start()
    {
        hfsm.Context.InitHp();
        ui.UpdateHeartUI(hfsm.Context.CurrentHp);

        hfsm.Context.onLanternToggled += OnLanternToggled; // 콜백 등록
    }

    private void OnDestroy()
    {
        hfsm.Context.onLanternToggled -= OnLanternToggled; // 메모리 누수 방지
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

    private void OnLanternToggled(bool isOn)
    {
        if (enemyData == null) return;
        enemyData.detectionRange = isOn ? lanternDetectionRange : defaultDetectionRange;
    }

    public void OnKeyPickup()
    {
        hfsm.Context.AddKey();
        ui.UpdateKeyUI(hfsm.Context.KeyCount, hfsm.Context.KeyRequired);
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

    public void TakeDamage(int amount = 1)
    {
        hfsm.Context.TakeDamage(amount);
        ui.UpdateHeartUI(hfsm.Context.CurrentHp);
    }
}