using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public PlayerController player;
    public Door door;
    public bool gameOver;

    private float gameStartTime;
    private bool isTimerRunning;

    // 일시정지 시간 제외한 실제 경과 시간
    public float ElapsedTime { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        NavMeshHelper.Initialize();
    }

    private void Update()
    {
        if (isTimerRunning)
            ElapsedTime += Time.deltaTime; // timeScale 0이면 안 증가
    }

    private void OnEnable() => SceneManager.sceneLoaded += OnSceneLoaded;
    private void OnDisable() => SceneManager.sceneLoaded -= OnSceneLoaded;

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        NavMeshHelper.Initialize();

        // 게임씬 진입 시 타이머 초기화 + 시작
        if (scene.buildIndex == 1)
        {
            ElapsedTime = 0f;
            isTimerRunning = true;
            gameOver = false;
        }
        else
        {
            isTimerRunning = false;
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        isTimerRunning = false; // 일시정지 중 타이머 정지
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        isTimerRunning = true;  // 재개 시 타이머 재시작
    }

    public void StopTimer()
    {
        isTimerRunning = false; // 클리어/게임오버 시 호출
    }

    // 포맷: 02:35.47
    public string GetFormattedTime()
    {
        int minutes = (int)(ElapsedTime / 60f);
        int seconds = (int)(ElapsedTime % 60f);
        int milliseconds = (int)((ElapsedTime % 1f) * 100f);
        return $"{minutes:00}:{seconds:00}.{milliseconds:00}";
    }
}