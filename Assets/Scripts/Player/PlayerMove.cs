using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMove : MonoBehaviour
{
    #region Movement
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float sprintMultiplier = 1.5f;

    public Vector2 moveInput;
    public bool sprinting;
    #endregion

    #region Jump & Gravity
    [Header("Jump")]
    public float jumpHeight = 1.5f;
    public float gravity = -9.81f;
    public float groundedGravity = -2f;

    public float verticalVelocity;
    #endregion

    #region Super Jump (Charge)
    [Header("Super Jump")]
    public float chargeTime = 2f;
    public float superJumpMultiplier = 20f;

    public float chargeTimer;
    public bool isCharging;

    public float ChargeRatio => isCharging ? chargeTimer / chargeTime : 0f;
    public bool IsCharging => isCharging;
    #endregion

    #region Input State
    private bool jumpPressed;
    private bool jumpTriggered;

    public bool JumpPressed => jumpPressed;

    public void SetJumpPressed(bool value)
    {
        if (value)
            jumpTriggered = true;

        jumpPressed = value;
    }

    public bool ConsumeJumpTriggered()
    {
        if (!jumpTriggered) return false;

        jumpTriggered = false;
        return true;
    }
    #endregion

    #region Components
    public CharacterController controller;
    #endregion

    #region State Machine
    private StateMachine stateMachine;

    public GroundedState groundedState;
    public AirState airState;
    public ChargeState chargeState;
    #endregion

    #region Unity Lifecycle
    private void Awake()
    {
        controller = GetComponent<CharacterController>();

        stateMachine = new StateMachine();

        groundedState = new GroundedState(this, stateMachine);
        airState = new AirState(this, stateMachine);
        chargeState = new ChargeState(this, stateMachine);
    }

    private void Start()
    {
        stateMachine.ChangeState(groundedState);
    }

    public void UpdateState()
    {
        stateMachine.Update();
    }
    #endregion

    #region Public API (Input ¿¬°á)
    public void SetMoveInput(Vector2 input) => moveInput = input;
    public void SetSprint(bool value) => sprinting = value;
    #endregion

    #region Physics
    public void ApplyGravity()
    {
        if (controller.isGrounded && verticalVelocity < 0f)
            verticalVelocity = groundedGravity;

        if ((controller.collisionFlags & CollisionFlags.Above) != 0 && verticalVelocity > 0f)
            verticalVelocity = 0f;

        verticalVelocity += gravity * Time.deltaTime;
    }

    public void Move()
    {
        Vector3 moveDir = Vector3.ClampMagnitude(
            transform.right * moveInput.x + transform.forward * moveInput.y, 1f);

        float speed = sprinting && controller.isGrounded
            ? moveSpeed * sprintMultiplier
            : moveSpeed;

        Vector3 velocity = moveDir * speed;
        velocity.y = verticalVelocity;

        controller.Move(velocity * Time.deltaTime);
    }
    #endregion

    #region Actions
    public void Jump(float multiplier)
    {
        float finalJumpHeight = jumpHeight * multiplier;
        verticalVelocity = Mathf.Sqrt(finalJumpHeight * -2f * gravity);
    }
    #endregion
}