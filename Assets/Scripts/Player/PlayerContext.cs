// PlayerContext.cs
using UnityEngine;

public class PlayerContext
{
    // 컴포넌트 참조
    public CharacterController controller;
    public Transform transform;

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