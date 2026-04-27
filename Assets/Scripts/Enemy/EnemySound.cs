using UnityEngine;
using UnityEngine.AI;

public class EnemySound : MonoBehaviour
{
    [SerializeField] private AudioClip footstepClip;
    [SerializeField] private AudioClip attackClip;

    [SerializeField] private float footstepVolume = 0.5f;
    [SerializeField] private float attackVolume = 1f;

    private NavMeshAgent agent;
    private bool isPlayingFootstep = false;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        HandleFootstep();
    }

    private void HandleFootstep()
    {
        bool isMoving = agent != null && agent.enabled && agent.velocity.magnitude > 0.5f;

        if (isMoving && !isPlayingFootstep)
        {
            isPlayingFootstep = true;
            SoundManager.Instance?.PlayAt(footstepClip,transform.position,footstepVolume);
        }
    }

    public void PlayAttack()
    {
        SoundManager.Instance?.PlayAt(attackClip, transform.position, attackVolume);
    }
}