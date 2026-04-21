using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimation : MonoBehaviour
{
    private Animator animator;
    private PlayerMove playerMove;

    void Start()
    {
        animator = GetComponent<Animator>();
        playerMove = GetComponentInParent<PlayerMove>();
    }

    void Update()
    {
        if (animator == null || playerMove == null)
            return;

        UpdateAnimation();
    }

    private void UpdateAnimation()
    {
        float speed = playerMove.moveInput.magnitude;
        bool isGrounded = playerMove.controller.isGrounded;
        float verticalVelocity = playerMove.verticalVelocity;

        // 파라미터 세팅
        animator.SetFloat("Speed", speed);
        animator.SetBool("IsGrounded", isGrounded);
        animator.SetFloat("VerticalVelocity", verticalVelocity);
    }
}