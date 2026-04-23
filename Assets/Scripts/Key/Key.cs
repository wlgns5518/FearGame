using UnityEngine;

public class Key : MonoBehaviour
{
    private PlayerUI playerUI;


    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        playerUI = other.GetComponent<PlayerUI>();
        playerUI?.AddKey();
        Destroy(gameObject);
    }
}