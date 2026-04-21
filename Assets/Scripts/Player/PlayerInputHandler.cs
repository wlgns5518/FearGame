using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    private PlayerMove move;
    private PlayerLook look;

    private PlayerInput playerInput;
    private InputAction sprintAction;
    private InputAction jumpAction;

    private void Awake()
    {
        move = GetComponent<PlayerMove>();
        look = GetComponent<PlayerLook>();

        playerInput = GetComponent<PlayerInput>();
        sprintAction = playerInput.actions["Sprint"];
        jumpAction = playerInput.actions["Jump"];
    }

    private void OnEnable()
    {
        sprintAction.performed += OnSprintPerformed;
        sprintAction.canceled += OnSprintCanceled;

        jumpAction.performed += OnJumpPerformed;
        jumpAction.canceled += OnJumpCanceled;
    }

    private void OnDisable()
    {
        sprintAction.performed -= OnSprintPerformed;
        sprintAction.canceled -= OnSprintCanceled;

        jumpAction.performed -= OnJumpPerformed;
        jumpAction.canceled -= OnJumpCanceled;
    }

    public void OnMove(InputValue value)
    {
        move.SetMoveInput(value.Get<Vector2>());
    }

    public void OnLook(InputValue value)
    {
        look.SetLookInput(value.Get<Vector2>());
    }

    private void OnSprintPerformed(InputAction.CallbackContext ctx)
    {
        move.SetSprint(true);
    }

    private void OnSprintCanceled(InputAction.CallbackContext ctx)
    {
        move.SetSprint(false);
    }

    private void OnJumpPerformed(InputAction.CallbackContext ctx)
    {
        move.SetJumpPressed(true);
    }

    private void OnJumpCanceled(InputAction.CallbackContext ctx)
    {
        move.SetJumpPressed(false);
    }
}