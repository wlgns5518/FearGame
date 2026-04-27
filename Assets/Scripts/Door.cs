using UnityEngine;

public class Door : MonoBehaviour
{
    private PlayerController player;
    private bool isTriggered = false;
    private void Awake()
    {
        GameManager.Instance.door = this;
    }

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
        player = obj.GetComponent<PlayerController>();

        if (player.HasEnoughKeys())
        {
            isTriggered = true;
            player.UseKeys();
            player.ui.OnPopUpOpen("이겼습니다.");
        }
        else
        {
            player.ui.OnPopUpOpen("열쇠가 부족합니다.");
        }
    }
}