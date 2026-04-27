using UnityEngine;

public class KeySound : MonoBehaviour
{
    [SerializeField] private AudioClip pickupSound;
    [SerializeField] private float volume = 1f;

    public void PlayPickupSound()
    {
        if (pickupSound != null)
            SoundManager.Instance?.Play(pickupSound, volume);
    }
}