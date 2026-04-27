// PlayerInputHandler.cs
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.LowLevel;

public class PlayerInputHandler : MonoBehaviour
{
    private PlayerHFSM hfsm;
    private PlayerLook look;
    private PlayerInput playerInput;
    private InputAction sprintAction;
    private InputAction jumpAction;
    private InputAction lanternAction; // 추가

    private void Awake()
    {
        hfsm = GetComponent<PlayerHFSM>();
        look = GetComponent<PlayerLook>();
        playerInput = GetComponent<PlayerInput>();
        sprintAction = playerInput.actions["Sprint"];
        jumpAction = playerInput.actions["Jump"];
        lanternAction = playerInput.actions["Lantern"]; // 추가
    }

    private void OnEnable()
    {
        sprintAction.performed += OnSprintPerformed;
        sprintAction.canceled += OnSprintCanceled;
        jumpAction.performed += OnJumpPerformed;
        jumpAction.canceled += OnJumpCanceled;
        lanternAction.performed += OnLanternPerformed; // 추가
    }

    private void OnDisable()
    {
        sprintAction.performed -= OnSprintPerformed;
        sprintAction.canceled -= OnSprintCanceled;
        jumpAction.performed -= OnJumpPerformed;
        jumpAction.canceled -= OnJumpCanceled;
        lanternAction.performed -= OnLanternPerformed; // 추가
    }

    private void OnLanternPerformed(InputAction.CallbackContext ctx)
        => hfsm.Context.ToggleLantern(); // 추가

    public void OnMove(InputValue value)
    {
        hfsm.Context.moveInput = value.Get<Vector2>();
    }

    public void OnLook(InputValue value)
    {
        look.SetLookInput(value.Get<Vector2>());
    }

    private void OnSprintPerformed(InputAction.CallbackContext ctx)
        => hfsm.Context.sprinting = true;

    private void OnSprintCanceled(InputAction.CallbackContext ctx)
        => hfsm.Context.sprinting = false;

    private void OnJumpPerformed(InputAction.CallbackContext ctx)
        => hfsm.Context.SetJumpPressed(true);

    private void OnJumpCanceled(InputAction.CallbackContext ctx)
        => hfsm.Context.SetJumpPressed(false);
}