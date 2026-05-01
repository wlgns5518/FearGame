using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingPanel : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private Slider sensitivitySlider;
    [SerializeField] private GameObject leaveGameButton;

    private const string VolumeKey = "Volume";
    private const string SensitivityKey = "Sensitivity";

    private void OnEnable()
    {
        volumeSlider?.onValueChanged.AddListener(OnVolumeChanged);
        sensitivitySlider?.onValueChanged.AddListener(OnSensitivityChanged);
        Refresh(); // 열릴 때마다 최신값 반영
    }

    private void OnDisable()
    {
        volumeSlider?.onValueChanged.RemoveListener(OnVolumeChanged);
        sensitivitySlider?.onValueChanged.RemoveListener(OnSensitivityChanged);
    }

    private void OnDestroy()
    {
        volumeSlider?.onValueChanged.RemoveListener(OnVolumeChanged);
        sensitivitySlider?.onValueChanged.RemoveListener(OnSensitivityChanged);
    }

    // ======================
    // 값 갱신
    // ======================
    public void Refresh()
    {
        if (volumeSlider != null)
            volumeSlider.value = PlayerPrefs.GetFloat(VolumeKey, 1f);

        if (sensitivitySlider != null)
            sensitivitySlider.value = PlayerPrefs.GetFloat(SensitivityKey, 0.5f);

        if (leaveGameButton != null)
            leaveGameButton.SetActive(SceneManager.GetActiveScene().buildIndex == 1);
    }

    // ======================
    // 슬라이더
    // ======================
    private void OnVolumeChanged(float value)
    {
        PlayerPrefs.SetFloat(VolumeKey, value);
        SoundManager.Instance?.ApplyVolume();
    }

    private void OnSensitivityChanged(float value)
    {
        PlayerPrefs.SetFloat(SensitivityKey, value);
        GameManager.Instance?.player?.look?.ApplySensitivity();
    }

    // ======================
    // 버튼
    // ======================
    public void OnClickClose()
    {
        UIManager.Instance.CloseSettingPanel();
    }

    public void OnClickReturnToLobby()
    {
        UIManager.Instance.ReturnToLobby();
    }
}