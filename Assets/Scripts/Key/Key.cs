// Key.cs
using UnityEngine;

public class Key : MonoBehaviour
{
    [SerializeField] private int keyIndex;
    [SerializeField] private float pickupRadius = 1.5f;
    [SerializeField] private float checkInterval = 0.1f; // 0.1초마다 체크

    private void Start()
    {
        InvokeRepeating(nameof(CheckPickup), 0f, checkInterval);
    }

    private void CheckPickup()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, pickupRadius);

        foreach (Collider col in colliders)
        {
            if (!col.CompareTag("Player")) continue;

            Destroy(gameObject);
            return;
        }
    }
}