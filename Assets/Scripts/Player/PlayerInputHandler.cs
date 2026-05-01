using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    private PlayerHFSM hfsm;
    private PlayerLook look;
    private PlayerInput playerInput;

    private InputAction sprintAction;
    private InputAction jumpAction;
    private InputAction lanternAction;
    private InputAction pauseAction;

    private void Awake()
    {
        hfsm = GetComponent<PlayerHFSM>();
        look = GetComponent<PlayerLook>();
        playerInput = GetComponent<PlayerInput>();

        sprintAction = playerInput.actions["Sprint"];
        jumpAction = playerInput.actions["Jump"];
        lanternAction = playerInput.actions["Lantern"];
        pauseAction = playerInput.actions["Pause"];
    }

    private void OnEnable()
    {
        sprintAction.performed += OnSprintPerformed;
        sprintAction.canceled += OnSprintCanceled;
        jumpAction.performed += OnJumpPerformed;
        jumpAction.canceled += OnJumpCanceled;
        lanternAction.performed += OnLanternPerformed;
        pauseAction.performed += OnPausePerformed;
    }

    private void OnDisable()
    {
        sprintAction.performed -= OnSprintPerformed;
        sprintAction.canceled -= OnSprintCanceled;
        jumpAction.performed -= OnJumpPerformed;
        jumpAction.canceled -= OnJumpCanceled;
        lanternAction.performed -= OnLanternPerformed;
        pauseAction.performed -= OnPausePerformed;
    }

    // 입력 감지만  판단은 PlayerUI가 함
    private void OnPausePerformed(InputAction.CallbackContext ctx)
        => hfsm.Context.onPauseToggled?.Invoke();

    private void OnLanternPerformed(InputAction.CallbackContext ctx)
        => hfsm.Context.ToggleLantern();

    public void OnMove(InputValue value)
        => hfsm.Context.moveInput = value.Get<Vector2>();

    public void OnLook(InputValue value)
        => look.SetLookInput(value.Get<Vector2>());

    private void OnSprintPerformed(InputAction.CallbackContext ctx)
        => hfsm.Context.sprinting = true;
    private void OnSprintCanceled(InputAction.CallbackContext ctx)
        => hfsm.Context.sprinting = false;
    private void OnJumpPerformed(InputAction.CallbackContext ctx)
        => hfsm.Context.SetJumpPressed(true);
    private void OnJumpCanceled(InputAction.CallbackContext ctx)
        => hfsm.Context.SetJumpPressed(false);
}