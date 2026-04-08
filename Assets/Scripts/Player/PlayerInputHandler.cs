using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    private PlayerMove move;
    private PlayerLook look;

    private void Awake()
    {
        move = GetComponent<PlayerMove>();
        look = GetComponent<PlayerLook>();
    }

    public void OnMove(InputValue value)
    {
        move.SetMoveInput(value.Get<Vector2>());
    }

    public void OnLook(InputValue value)
    {
        look.SetLookInput(value.Get<Vector2>());
    }

    public void OnJump(InputValue value)
    {
        if (value.isPressed)
        {
            move.Jump();
        }
    }
}