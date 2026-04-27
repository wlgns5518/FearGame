using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private Slider chargeSlider;
    [SerializeField] private TextMeshProUGUI keyText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private GameObject popUpPanel;
    [SerializeField] private GameObject[] hearts;
    private bool isFirstClose = true;

    private void Start()
    {
        OnPopUpOpen("[조작 방법]\r\nWASD : 이동\r\nShift : 대쉬\r\nSpace : 점프\r\nShift + Space : 슈퍼 점프\r\n1 : 램프 ON/OFF");
    }

    public void UpdateChargeUI(float ratio, bool isCharging)
    {
        if (isCharging)
        {
            chargeSlider.gameObject.SetActive(true);
            chargeSlider.value = ratio;
        }
        else
        {
            chargeSlider.value = 0f;
            chargeSlider.gameObject.SetActive(false);
        }
    }

    public void UpdateKeyUI(int current, int required)
    {
        keyText.text = $"Key : {current} / {required}";
    }

    public void UpdateHeartUI(int hp)
    {
        for (int i = 0; i < hearts.Length; i++)
            hearts[i].SetActive(i < hp);

        if (hp <= 0)
            OnPopUpOpen("게임 오버!");
    }

    public void OnPopUpClose()
    {
        if (popUpPanel != null)
            popUpPanel.SetActive(false);
        GameManager.Instance.ResumeGame();
        PlayerLook.LockCursor();
        if (isFirstClose)
        {
            isFirstClose = false;
            OnPopUpOpen("좀비를 피해 열쇠 3개를 모으세요");
        }
    }

    public void OnPopUpOpen(string text)
    {
        popUpPanel.SetActive(true);
        PlayerLook.UnlockCursor();
        GameManager.Instance.PauseGame();
        descriptionText.text = text;
    }
}