// PlayerData.cs
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Player/PlayerData")]
public class PlayerData : ScriptableObject
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float sprintMultiplier = 1.5f;

    [Header("Jump")]
    public float jumpHeight = 1.5f;
    public float gravity = -9.81f;
    public float groundedGravity = -2f;

    [Header("Charge")]
    public float chargeTime = 2f;
    public float superJumpMultiplier = 20f;
}