// Key.cs
using UnityEngine;

public class Key : MonoBehaviour
{
    [SerializeField] private int keyIndex;
    [SerializeField] private float checkInterval = 0.1f;

    private void Start()
    {
        InvokeRepeating(nameof(CheckPickup), 0f, checkInterval);
    }

    private void CheckPickup()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 1.5f);

        foreach (Collider col in colliders)
        {
            if (!col.CompareTag("Player")) continue;

            col.GetComponent<PlayerUI>().AddKey();
            Destroy(gameObject);
            return;
        }
    }
}