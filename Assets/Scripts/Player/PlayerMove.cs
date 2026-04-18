using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMove : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float sprintMultiplier = 2f;

    [Header("Jump")]
    [SerializeField] private float jumpHeight = 1.5f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float groundedGravity = -2f;

    [Header("Super Jump")]
    [SerializeField] private float chargeTime = 2f;
    [SerializeField] private float superJumpMultiplier = 20f;

    private CharacterController controller;
    private Vector2 moveInput;
    private float verticalVelocity;
    private bool jumpRequested;
    private bool sprinting;

    private bool isCharging;
    private float chargeTimer;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    public void SetMoveInput(Vector2 input) => moveInput = input;

    public void SetSprint(bool value)
    {
        sprinting = value;

        if (!value && isCharging)
        {
            jumpRequested = true;
            isCharging = false;
        }
    }

    public void Jump()
    {
        if (sprinting)
        {
            isCharging = true;
            chargeTimer = 0f;
        }
        else
        {
            jumpRequested = true;
        }
    }

    public void StopJumpCharge()
    {
        if (isCharging)
        {
            jumpRequested = true;
            isCharging = false;
        }
    }

    private void Update()
    {
        HandleCharge();
        Move();
    }

    private void HandleCharge()
    {
        if (!isCharging) return;

        chargeTimer += Time.deltaTime;

        if (chargeTimer >= chargeTime)
        {
            chargeTimer = chargeTime;
        }
    }

    private void Move()
    {
        bool isGrounded = controller.isGrounded;

        if (isGrounded && verticalVelocity < 0f)
            verticalVelocity = groundedGravity;

        if (isGrounded && jumpRequested)
        {
            float finalJumpHeight = jumpHeight;

            if (chargeTimer > 0f)
            {
                float chargeRatio = Mathf.Clamp01(chargeTimer / chargeTime);
                float jumpMultiplier = Mathf.Lerp(1f, superJumpMultiplier, chargeRatio);
                finalJumpHeight *= jumpMultiplier;
            }

            verticalVelocity = Mathf.Sqrt(finalJumpHeight * -2f * gravity);

            chargeTimer = 0f;
            isCharging = false;
        }

        jumpRequested = false;

        // 머리를 박았는지 확인
        if ((controller.collisionFlags & CollisionFlags.Above) != 0 && verticalVelocity > 0f)
        {
            verticalVelocity = 0f; // 머리를 박으면 떨어지도록 설정
        }

        verticalVelocity += gravity * Time.deltaTime;

        Vector3 moveDir = transform.right * moveInput.x + transform.forward * moveInput.y;
        moveDir = Vector3.ClampMagnitude(moveDir, 1f);

        float speed = sprinting ? moveSpeed * sprintMultiplier : moveSpeed;

        Vector3 velocity = moveDir * speed;
        velocity.y = verticalVelocity;

        controller.Move(velocity * Time.deltaTime);
    }
}