using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private Slider chargeSlider;
    [SerializeField] private TextMeshProUGUI keyText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private GameObject popUpPanel;
    [SerializeField] private GameObject[] hearts;

    private bool isFirstClose = true;

    public bool IsPopUpOpen => popUpPanel != null && popUpPanel.activeSelf;

    private void Start()
    {
        OnPopUpOpen("[조작 방법]\nWASD : 이동\nShift : 대쉬\nSpace : 점프\nShift + Space : 슈퍼 점프\n1 : 램프 ON/OFF");
    }

    // ========================
    // HUD 업데이트
    // ========================
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
    }

    // ========================
    // 팝업
    // ========================
    public void OnPopUpOpen(string text)
    {
        popUpPanel.SetActive(true);
        descriptionText.text = text;
        PlayerLook.UnlockCursor();
        GameManager.Instance.PauseGame();
    }

    public void OnPopUpClose()
    {
        if(GameManager.Instance.gameOver)
        {
            SceneManager.LoadScene(0);
        }
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

    // ========================
    // ESC 토글 (PlayerHFSM이 연결)
    // ========================
    public void OnPauseToggle()
    {
        // 팝업이 열려있으면 ESC 무시
        if (IsPopUpOpen) return;
        if (UIManager.Instance == null) return;

        if (UIManager.Instance.IsSettingPanelOpen)
            UIManager.Instance.CloseSettingPanel();
        else
            UIManager.Instance.OpenSettingPanel();
    }
}