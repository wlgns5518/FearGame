// PlayerRespawn.cs
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    private Vector3 spawnPosition;
    private CharacterController controller;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        spawnPosition = transform.position;
    }

    public void Respawn()
    {
        controller.enabled = false;
        transform.position = spawnPosition;
        controller.enabled = true;
    }
}