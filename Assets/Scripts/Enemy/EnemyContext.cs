using UnityEngine;
using UnityEngine.AI;

public class EnemyContext
{
    public NavMeshAgent agent;
    public Transform transform;
    public Transform player;
    public EnemyData data;
    public CharacterController controller;
    public Animator animator;
    public EnemySound sound;

    public float waitTimer;
    public float attackTimer;
    public float verticalVelocity;

    // 캐싱 (매 호출마다 new 방지)
    private static readonly float[] WarpRadii = { 1f, 2f, 3f, 5f };

    // ========================
    // 거리 계산
    // ========================
    private float SqrDistanceToPlayer =>
        (transform.position - player.position).sqrMagnitude;

    // sqrt 필요한 곳에서만 사용
    public float DistanceToPlayer =>
        (transform.position - player.position).magnitude; // Vector3.Distance 제거

    public bool PlayerInDetectionRange =>
        SqrDistanceToPlayer <= data.detectionRange * data.detectionRange;

    public bool PlayerInAttackRange =>
        SqrDistanceToPlayer <= data.attackRange * data.attackRange;

    public bool PlayerIsAbove =>
        player.position.y > transform.position.y + 1f;

    // ========================
    // 이동
    // ========================
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

    // ========================
    // 장애물 감지
    // ========================
    public bool ObstacleAhead()
    {
        return Physics.Raycast(
            transform.position + Vector3.up * 0.5f,
            transform.forward,
            data.jumpObstacleDistance
        );
    }

    // ========================
    // 점프 / 중력
    // ========================
    public void Jump(float multiplier = 1f)
    {
        float finalJumpHeight = data.jumpHeight * multiplier;
        verticalVelocity = Mathf.Sqrt(finalJumpHeight * -2f * data.gravity);
    }

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

    // ========================
    // NavMesh 복구
    // ========================
    public bool WarpToNavMesh()
    {
        NavMeshHit hit;
        foreach (float radius in WarpRadii) // static readonly로 캐싱
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
        Debug.LogWarning("현재 위치 근처에 NavMesh가 없습니다.");
        return false;
    }
}