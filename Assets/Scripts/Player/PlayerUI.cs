using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private Slider chargeSlider;
    [SerializeField] private TextMeshProUGUI keyText;
    [SerializeField] private GameObject winPanel;

    private int keyCount = 0;
    private const int RequiredKeys = 3;

    private void Start()
    {
        UpdateKeyText();
        if (winPanel != null)
            winPanel.SetActive(false);
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

    public void AddKey()
    {
        keyCount++;
        UpdateKeyText();
    }

    public bool HasEnoughKeys() => keyCount >= RequiredKeys;

    public void UseKeys()
    {
        keyCount -= RequiredKeys;
        UpdateKeyText();
    }

    private void UpdateKeyText()
    {
        keyText.text = $"Key : {keyCount} / {RequiredKeys}";
    }

    public void ShowWinScreen()
    {
        if (winPanel != null)
            winPanel.SetActive(true);
    }
}