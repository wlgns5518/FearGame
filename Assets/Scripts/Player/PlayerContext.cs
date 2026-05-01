using UnityEngine;

public class PlayerContext
{
    // 컴포넌트 참조
    public CharacterController controller;
    public Transform transform;
    public Animator animator;
    public PlayerSound sound; // 캐싱 추가

    // 데이터
    public PlayerData data;

    // 입력
    public Vector2 moveInput;
    public bool sprinting;
    private bool jumpPressed;
    private bool jumpTriggered;
    public bool JumpPressed => jumpPressed;

    // 물리
    public float verticalVelocity;

    // 차지
    public float chargeTimer;
    public bool isCharging;
    public float ChargeRatio => isCharging ? chargeTimer / data.chargeTime : 0f;

    // 키
    private int keyCount = 0;
    private const int RequiredKeys = 3;
    public int KeyCount => keyCount;
    public int KeyRequired => RequiredKeys;
    public bool HasEnoughKeys() => keyCount >= RequiredKeys;
    public void AddKey() => keyCount++;

    // 랜턴
    public bool lanternOn = false;
    public GameObject lantern;

    // HP
    private int currentHp;
    private int maxHp = 3;
    public int CurrentHp => currentHp;
    public int MaxHp => maxHp;
    public void InitHp() => currentHp = maxHp;

    public void TakeDamage(int amount = 1)
    {
        currentHp = Mathf.Clamp(currentHp - amount, 0, maxHp);
        if (currentHp <= 0)
        {
            onPopUpOpen?.Invoke("게임 오버!");
            GameManager.Instance.gameOver = true;
        }
    }

    // ========================
    // 이벤트 (UI 및 PlayerHFSM에서 사용)
    // ========================
    public System.Action<string> onPopUpOpen;
    public System.Action onPopUpClose;
    public System.Action<bool> onLanternToggled;
    public System.Action onPauseToggled;

    // ========================
    // 랜턴
    // ========================
    public void ToggleLantern()
    {
        lanternOn = !lanternOn;

        if (lanternOn)
        {
            animator.SetLayerWeight(1, 1f);
            animator.Play("Lamp", 1);
            lantern.SetActive(true);
        }
        else
        {
            animator.SetLayerWeight(1, 0f);
            lantern.SetActive(false);
        }

        onLanternToggled?.Invoke(lanternOn);
    }

    // ========================
    // 점프
    // ========================
    public void SetJumpPressed(bool value)
    {
        if (value) jumpTriggered = true;
        jumpPressed = value;
    }

    public bool ConsumeJumpTriggered()
    {
        if (!jumpTriggered) return false;
        jumpTriggered = false;
        return true;
    }

    // ========================
    // 물리
    // ========================
    public void ApplyGravity()
    {
        if (controller.isGrounded && verticalVelocity < 0f)
            verticalVelocity = data.groundedGravity;

        if ((controller.collisionFlags & CollisionFlags.Above) != 0 && verticalVelocity > 0f)
            verticalVelocity = 0f;

        verticalVelocity += data.gravity * Time.deltaTime;
    }

    public void Move()
    {
        Vector3 moveDir = Vector3.ClampMagnitude(
            transform.right * moveInput.x + transform.forward * moveInput.y, 1f);

        float speed = sprinting && controller.isGrounded
            ? data.moveSpeed * data.sprintMultiplier
            : data.moveSpeed;

        Vector3 velocity = moveDir * speed;
        velocity.y = verticalVelocity;
        controller.Move(velocity * Time.deltaTime);
    }

    public void Jump(float multiplier = 1f)
    {
        float finalJumpHeight = data.jumpHeight * multiplier;
        verticalVelocity = Mathf.Sqrt(finalJumpHeight * -2f * data.gravity);
    }
}