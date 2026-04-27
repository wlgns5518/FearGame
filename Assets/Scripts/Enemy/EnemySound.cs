using UnityEngine;
public class EnemySound : MonoBehaviour
{
    [SerializeField] private AudioClip footstepClip;
    [SerializeField] private AudioClip attackClip;
    [SerializeField] private float footstepVolume = 1f;
    [SerializeField] private float attackVolume = 1f;
    [SerializeField] private float footstepInterval = 0.4f;

    private float footstepTimer = 0f;

    public void PlayFootstep()
    {
        footstepTimer -= Time.deltaTime;
        if (footstepTimer <= 0f)
        {
            SoundManager.Instance?.PlayAt(footstepClip, transform.position, footstepVolume);
            footstepTimer = footstepInterval;
        }
    }

    public void PlayAttack()
    {
        SoundManager.Instance?.Play(attackClip, attackVolume);
    }
}