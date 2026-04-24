using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private Slider chargeSlider;
    [SerializeField] private TextMeshProUGUI keyText;
    [SerializeField] private TextMeshProUGUI descriptionText;

    private void Start()
    {
        ShowText("좀비를 피해 열쇠3개를 구하시오");
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

    public void ShowText(string message)
    {
        StopAllCoroutines(); // 이전 실행 중지 (중복 방지)

        descriptionText.text = message;
        descriptionText.gameObject.SetActive(true);

        StartCoroutine(HideTextAfterTime(3f));
    }

    private IEnumerator HideTextAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        descriptionText.gameObject.SetActive(false);
    }
}