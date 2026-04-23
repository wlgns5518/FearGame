using UnityEngine;

public class Door : MonoBehaviour
{
    private PlayerUI playerUI;
    private bool isTriggered = false;

    // CharacterController가 부딪혔을 때 감지
    private void OnCollisionEnter(Collision collision)
    {
        TryEnter(collision.gameObject);
    }

    // 혹시 Trigger로 설정된 경우도 커버
    private void OnTriggerEnter(Collider other)
    {
        TryEnter(other.gameObject);
    }

    private void TryEnter(GameObject obj)
    {
        if (isTriggered) return;
        if (!obj.CompareTag("Player")) return;
        playerUI = obj.GetComponent<PlayerUI>();

        if (playerUI.HasEnoughKeys())
        {
            isTriggered = true;
            playerUI.UseKeys();
            playerUI.ShowWinScreen();
        }
        else
        {
            Debug.Log("열쇠가 부족합니다.");
        }
    }
}