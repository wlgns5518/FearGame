// PlayerSound.cs
using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    [Header("Footstep")]
    [SerializeField] private AudioClip[] footstepClips;
    [SerializeField] private float footstepInterval = 0.4f;
    [SerializeField] private float sprintFootstepInterval = 0.25f;

    [Header("Jump")]
    [SerializeField] private AudioClip jumpClip;
    [SerializeField] private AudioClip landClip;

    private PlayerHFSM hfsm;
    private float footstepTimer;
    private bool wasGrounded;

    private void Start()
    {
        hfsm = GetComponent<PlayerHFSM>();
    }

    private void Update()
    {
        if (hfsm == null) return;

        PlayerContext ctx = hfsm.Context;
        bool isGrounded = ctx.controller.isGrounded;
        float speed = ctx.moveInput.magnitude;

        HandleFootstep(ctx, isGrounded, speed);
        HandleLanding(isGrounded);

        wasGrounded = isGrounded;
    }

    private void HandleFootstep(PlayerContext ctx, bool isGrounded, float speed)
    {
        if (!isGrounded || speed <= 0.01f)
        {
            footstepTimer = 0f;
            return;
        }

        float interval = ctx.sprinting ? sprintFootstepInterval : footstepInterval;
        footstepTimer += Time.deltaTime;

        if (footstepTimer >= interval)
        {
            footstepTimer = 0f;
            PlayRandomFootstep();
        }
    }

    private void HandleLanding(bool isGrounded)
    {
        if (!wasGrounded && isGrounded)
            SoundManager.Instance.PlayAt(landClip, transform.position);
    }

    public void PlayJump()
    {
        SoundManager.Instance.PlayAt(jumpClip, transform.position);
    }

    private void PlayRandomFootstep()
    {
        if (footstepClips.Length == 0) return;
        AudioClip clip = footstepClips[Random.Range(0, footstepClips.Length)];
        SoundManager.Instance.PlayAt(clip, transform.position);
    }
}