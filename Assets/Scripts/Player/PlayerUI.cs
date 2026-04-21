using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private Slider chargeSlider;

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
}