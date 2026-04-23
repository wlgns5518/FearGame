// EnemyContext.cs
using UnityEngine;
using UnityEngine.AI;

public class EnemyContext
{
    public NavMeshAgent agent;
    public Transform transform;
    public Transform player;
    public EnemyData data;
    public CharacterController controller; // 점프용

    public float waitTimer;
    public float attackTimer;
    public float verticalVelocity;

    public float DistanceToPlayer =>
        Vector3.Distance(transform.position, player.position);

    public bool PlayerInDetectionRange =>
        DistanceToPlayer <= data.detectionRange;

    public bool PlayerInAttackRange =>
        DistanceToPlayer <= data.attackRange;

    // 플레이어가 자신보다 높은 위치에 있는지
    public bool PlayerIsAbove =>
        player.position.y > transform.position.y + 1f;

    public void MoveToPosition(Vector3 position, float speed)
    {
        if (!agent.enabled || !agent.isOnNavMesh) return;

        agent.speed = speed;
        agent.SetDestination(position);
    }

    public bool ReachedDestination()
    {
        if (!agent.enabled || !agent.isOnNavMesh) return false;

        return !agent.pathPending &&
               agent.remainingDistance <= agent.stoppingDistance;
    }

    public void Jump(float multiplier = 1f)
    {
        float finalJumpHeight = data.jumpHeight * multiplier;
        verticalVelocity = Mathf.Sqrt(finalJumpHeight * -2f * data.gravity);
    }

    public void ApplyGravity()
    {
        if (controller.isGrounded && verticalVelocity < 0f)
            verticalVelocity = data.groundedGravity;

        if ((controller.collisionFlags & CollisionFlags.Above) != 0 && verticalVelocity > 0f)
            verticalVelocity = 0f;

        verticalVelocity += data.gravity * Time.deltaTime;

        controller.Move(Vector3.up * verticalVelocity * Time.deltaTime);
    }

    public void ApplyGravityAndMove()
    {
        if (controller.isGrounded && verticalVelocity < 0f)
            verticalVelocity = data.groundedGravity;

        if ((controller.collisionFlags & CollisionFlags.Above) != 0 && verticalVelocity > 0f)
            verticalVelocity = 0f;

        verticalVelocity += data.gravity * Time.deltaTime;

        // 플레이어 방향으로 수평 이동
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        directionToPlayer.y = 0f;

        Vector3 velocity = directionToPlayer * data.chaseSpeed;
        velocity.y = verticalVelocity;

        controller.Move(velocity * Time.deltaTime);
    }

    // 장애물 감지
    public bool ObstacleAhead()
    {
        return Physics.Raycast(
            transform.position + Vector3.up * 0.5f,
            transform.forward,
            data.jumpObstacleDistance
        );
    }
    public bool IsPathBlocked()
    {
        if (!agent.enabled || !agent.isOnNavMesh) return false;

        return agent.pathStatus == NavMeshPathStatus.PathPartial
            || agent.pathStatus == NavMeshPathStatus.PathInvalid;
    }
}