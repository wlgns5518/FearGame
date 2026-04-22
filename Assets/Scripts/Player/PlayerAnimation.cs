// PlayerAnimation.cs
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimation : MonoBehaviour
{
    private Animator animator;
    private PlayerHFSM hfsm;
    private bool wasGrounded;

    private void Start()
    {
        animator = GetComponent<Animator>();
        hfsm = GetComponentInParent<PlayerHFSM>();
    }

    private void Update()
    {
        if (animator == null || hfsm == null) return;
        UpdateAnimation();
    }

    private void UpdateAnimation()
    {
        PlayerContext ctx = hfsm.Context;
        bool isGrounded = ctx.controller.isGrounded;
        float speed = ctx.moveInput.magnitude;

        if (!isGrounded)
        {
            if (wasGrounded && !isGrounded)
                animator.Play("Jump");
            else if (ctx.verticalVelocity < 0f)
                animator.Play("Falling");
        }
        else
        {
            if (speed <= 0.01f)
                animator.Play("Idle");
            else if (ctx.sprinting)
                animator.Play("Sprint");
            else
                animator.Play("Walk");
        }

        wasGrounded = isGrounded;
    }
}