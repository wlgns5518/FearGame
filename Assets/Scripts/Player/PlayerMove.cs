using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMove : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;

    [Header("Jump")]
    [SerializeField] private float jumpHeight = 1.5f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float groundedGravity = -2f;

    private CharacterController controller;
    private Vector2 moveInput;
    private float verticalVelocity;
    private bool jumpRequested;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    public void SetMoveInput(Vector2 input)
    {
        moveInput = input;
    }

    public void Jump()
    {
        jumpRequested = true;
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        bool isGrounded = controller.isGrounded;

        if (isGrounded && verticalVelocity < 0f)
        {
            verticalVelocity = groundedGravity;
        }

        if (isGrounded && jumpRequested)
        {
            verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        jumpRequested = false;
        verticalVelocity += gravity * Time.deltaTime;

        Vector3 moveDir = transform.right * moveInput.x + transform.forward * moveInput.y;
        moveDir = Vector3.ClampMagnitude(moveDir, 1f);

        Vector3 velocity = moveDir * moveSpeed;
        velocity.y = verticalVelocity;

        controller.Move(velocity * Time.deltaTime);
    }
}