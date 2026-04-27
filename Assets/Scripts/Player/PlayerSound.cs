using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    [Header("Jump")]
    [SerializeField] private AudioClip jumpClip;

    public void PlayJump()
    {
        SoundManager.Instance.PlayAt(jumpClip, transform.position);
    }
}