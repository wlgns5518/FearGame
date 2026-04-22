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
    private bool wasGrounded;

    private void UpdateAnimation()
    {
        float speed = playerMove.moveInput.magnitude;
        bool isGrounded = playerMove.controller.isGrounded;

        animator.SetFloat("Speed", speed);
        animator.SetBool("IsGrounded", isGrounded);

        // 점프 시작 순간 감지
        if (wasGrounded && !isGrounded)
        {
            animator.SetTrigger("Jump");
        }

        wasGrounded = isGrounded;
    }
}