// EnemyAnimation.cs
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EnemyAnimation : MonoBehaviour
{
    private Animator animator;
    private EnemyHFSM hfsm;

    private void Start()
    {
        animator = GetComponent<Animator>();
        hfsm = GetComponentInParent<EnemyHFSM>();
    }

    private void Update()
    {
        if (animator == null || hfsm == null) return;
        UpdateAnimation();
    }

    private void UpdateAnimation()
    {
        IState rootState = hfsm.CurrentRootState;

        if (rootState is EnemyPatrolState patrolState)
        {
            IState subState = patrolState.CurrentSubState;

            if (subState is EnemyWalkState)
                animator.Play("Walk");
            else if (subState is EnemyWaitState)
                animator.Play("Idle");
        }
        else if (rootState is EnemyChaseState chaseState)
        {
            IState subState = chaseState.CurrentSubState;

            if (subState is EnemyMoveState)
                animator.Play("Chase");
            else if (subState is EnemyAttackState)
                animator.Play("Attack");
            else if (subState is EnemyJumpState)
            {
                if (hfsm.Context.verticalVelocity > 0f)
                    animator.Play("Jump");
                else
                    animator.Play("Falling");
            }
        }
    }
}