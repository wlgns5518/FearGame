using UnityEngine;

public class Key : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        other.GetComponent<PlayerController>()?.OnKeyPickup();
        Destroy(gameObject);
    }
}