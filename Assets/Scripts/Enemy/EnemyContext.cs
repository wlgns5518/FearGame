using UnityEngine;
using UnityEngine.AI;

public class EnemyContext
{
    public NavMeshAgent agent;
    public Transform transform;
    public Transform player;
    public EnemyData data;
    public CharacterController controller;

    public float waitTimer;
    public float attackTimer;
    public float verticalVelocity;

    public float DistanceToPlayer =>
        Vector3.Distance(transform.position, player.position);
    public bool PlayerInDetectionRange =>
        DistanceToPlayer <= data.detectionRange;
    public bool PlayerInAttackRange =>
        DistanceToPlayer <= data.attackRange;
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
        if (agent.pathPending) return false;
        if (agent.remainingDistance > agent.stoppingDistance) return false;
        if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f) return false;
        return true;
    }

    public bool IsPathBlocked()
    {
        if (!agent.enabled || !agent.isOnNavMesh) return false;
        if (agent.pathPending) return false;
        return agent.pathStatus == NavMeshPathStatus.PathPartial
            || agent.pathStatus == NavMeshPathStatus.PathInvalid;
    }

    public void ResetPath()
    {
        if (agent.enabled && agent.isOnNavMesh)
            agent.ResetPath();
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

    // 점프
    public void Jump(float multiplier = 1f)
    {
        float finalJumpHeight = data.jumpHeight * multiplier;
        verticalVelocity = Mathf.Sqrt(finalJumpHeight * -2f * data.gravity);
    }

    // 점프 중 이동
    public void ApplyGravityAndMove()
    {
        if (controller.isGrounded && verticalVelocity < 0f)
            verticalVelocity = data.groundedGravity;
        if ((controller.collisionFlags & CollisionFlags.Above) != 0 && verticalVelocity > 0f)
            verticalVelocity = 0f;

        verticalVelocity += data.gravity * Time.deltaTime;

        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0f;
        Vector3 velocity = direction * data.chaseSpeed;
        velocity.y = verticalVelocity;
        controller.Move(velocity * Time.deltaTime);
    }

    // 착지 후 NavMesh 복귀
    public bool WarpToNavMesh()
    {
        float[] radii = { 1f, 2f, 3f, 5f };
        NavMeshHit hit;

        foreach (float radius in radii)
        {
            if (NavMesh.SamplePosition(transform.position, out hit, radius, NavMesh.AllAreas))
            {
                agent.enabled = false;
                controller.enabled = false;
                transform.position = hit.position;
                controller.enabled = true;
                agent.enabled = true;
                agent.Warp(hit.position);
                return true;
            }
        }

        Debug.LogWarning("착지 위치 근처에 NavMesh가 없습니다.");
        return false;
    }
}