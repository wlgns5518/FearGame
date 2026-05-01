// Door.cs - StopTimer 연동
using UnityEngine;

public class Door : MonoBehaviour
{
    private PlayerController player;
    private bool isTriggered = false;

    private void Awake()
    {
        GameManager.Instance.door = this;
    }

    private void OnCollisionEnter(Collision collision) => TryEnter(collision.gameObject);
    private void OnTriggerEnter(Collider other) => TryEnter(other.gameObject);

    private void TryEnter(GameObject obj)
    {
        if (isTriggered) return;
        if (!obj.CompareTag("Player")) return;

        player = obj.GetComponent<PlayerController>();

        if (player.HasEnoughKeys())
        {
            isTriggered = true;
            GameManager.Instance.StopTimer(); // 타이머 정지
            GameManager.Instance.gameOver = true;

            string timeText = $"이겼습니다!\n클리어 시간 : {GameManager.Instance.GetFormattedTime()}";
            player.ui.OnPopUpOpen(timeText);
        }
        else
        {
            player.ui.OnPopUpOpen("열쇠가 부족합니다.");
        }
    }
}