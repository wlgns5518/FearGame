// SoundManager.cs - 볼륨 관련 추가
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }
    [SerializeField] private int poolSize = 10;
    private readonly List<AudioSource> _pool = new();

    private float masterVolume = 1f; // 추가

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            BuildPool();
            ApplyVolume(); // 저장된 볼륨 초기 적용
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // SettingPanel에서 호출
    public void ApplyVolume()
    {
        masterVolume = PlayerPrefs.GetFloat("Volume", 1f);
        AudioListener.volume = masterVolume;
    }

    private void BuildPool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            var go = new GameObject($"SFX_Pool_{i}");
            go.transform.SetParent(transform);
            var src = go.AddComponent<AudioSource>();
            src.playOnAwake = false;
            _pool.Add(src);
        }
    }

    private AudioSource GetFreeSource()
    {
        foreach (var src in _pool)
            if (!src.isPlaying) return src;
        _pool[0].Stop();
        return _pool[0];
    }

    public void Play(AudioClip clip, float volume = 1f, float pitch = 1f)
    {
        if (clip == null) return;
        var src = GetFreeSource();
        src.transform.position = Vector3.zero;
        src.spatialBlend = 0f;
        src.pitch = pitch;
        src.PlayOneShot(clip, volume);
    }

    public void PlayAt(AudioClip clip, Vector3 position, float volume = 1f)
    {
        if (clip == null) return;
        var src = GetFreeSource();
        src.transform.position = position;
        src.spatialBlend = 1f;
        src.PlayOneShot(clip, volume);
    }
}