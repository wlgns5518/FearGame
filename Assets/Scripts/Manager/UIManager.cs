// UIManager.cs
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Prefabs")]
    [SerializeField] private GameObject settingPanelPrefab;

    [Header("Settings")]
    [SerializeField] private int gameSceneIndex = 1;

    private GameObject settingPanelInstance;
    private SettingPanel settingPanelComponent;
    private Canvas cachedCanvas;
    private int currentSceneIndex;

    public bool IsSettingPanelOpen
        => settingPanelInstance != null && settingPanelInstance.activeSelf;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable() => SceneManager.sceneLoaded += OnSceneLoaded;
    private void OnDisable() => SceneManager.sceneLoaded -= OnSceneLoaded;

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        currentSceneIndex = scene.buildIndex;
        CacheCanvas();
        ReparentSettingPanel();

        if (currentSceneIndex != gameSceneIndex)
            ApplyCursorState();
    }

    private void CacheCanvas()
    {
        cachedCanvas = FindAnyObjectByType<Canvas>();
    }

    private void ReparentSettingPanel()
    {
        if (settingPanelInstance == null || cachedCanvas == null) return;
        settingPanelInstance.transform.SetParent(cachedCanvas.transform, false);
    }

    private void ApplyCursorState()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void OpenSettingPanel()
    {
        if (settingPanelInstance == null)
        {
            settingPanelInstance = Instantiate(settingPanelPrefab);
            settingPanelComponent = settingPanelInstance.GetComponent<SettingPanel>();

            if (cachedCanvas != null)
                settingPanelInstance.transform.SetParent(cachedCanvas.transform, false);
        }

        settingPanelInstance.SetActive(true);
        settingPanelComponent?.Refresh();

        GameManager.Instance.PauseGame(); // 변경
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void CloseSettingPanel()
    {
        if (settingPanelInstance != null)
            settingPanelInstance.SetActive(false);

        GameManager.Instance.ResumeGame(); // 변경

        if (currentSceneIndex == gameSceneIndex)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            ApplyCursorState();
        }
    }

    public void StartGame()
    {
        GameManager.Instance.ResumeGame(); // 변경
        SceneManager.LoadScene(gameSceneIndex);
    }

    public void ReturnToLobby()
    {
        GameManager.Instance.ResumeGame(); // 변경
        SceneManager.LoadScene(0);
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}