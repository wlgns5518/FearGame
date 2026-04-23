// EnemyData.cs
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Enemy/EnemyData")]
public class EnemyData : ScriptableObject
{
    [Header("Movement")]
    public float moveSpeed = 3f;
    public float chaseSpeed = 5f;

    [Header("Detection")]
    public float detectionRange = 10f;
    public float attackRange = 2f;

    [Header("Patrol")]
    public float waitTime = 2f;
    public float patrolRadius = 10f;

    [Header("Attack")]
    public float attackCooldown = 1f;
    public float attackDamage = 10f;

    [Header("Jump")]
    public float jumpHeight = 2f;
    public float gravity = -9.81f;
    public float groundedGravity = -2f;
    public float superJumpMultiplier = 3f;

    [Header("Jump Trigger")]
    public float jumpObstacleDistance = 2f; // 장애물 감지 거리
}