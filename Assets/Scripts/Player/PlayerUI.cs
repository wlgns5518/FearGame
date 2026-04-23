// PlayerUI.cs
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private Slider chargeSlider;
    [SerializeField] private TextMeshProUGUI keyText;

    private int keyCount = 0;

    private void Start()
    {
        UpdateKeyText();
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

    private void UpdateKeyText()
    {
        keyText.text = $"Key : {keyCount}";
    }
}