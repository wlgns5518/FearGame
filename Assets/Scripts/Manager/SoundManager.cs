using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [SerializeField] private int poolSize = 10;

    private readonly List<AudioSource> _pool = new();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            BuildPool();
        }
        else
        {
            Destroy(gameObject);
        }
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

        // 풀이 가득 차면 첫 번째 강제 재사용
        _pool[0].Stop();
        return _pool[0];
    }

    /// <summary>2D 효과음 재생 (발소리 등)</summary>
    // SoundManager.cs
    public void Play(AudioClip clip, float volume = 1f, float pitch = 1f)
    {
        if (clip == null) return;
        var src = GetFreeSource();
        src.transform.position = Vector3.zero;
        src.spatialBlend = 0f; // 2D
        src.pitch = pitch;           // pitch 적용
        src.PlayOneShot(clip, volume);
    }

    /// <summary>3D 위치 효과음 재생 (점프, 폭발 등)</summary>
    public void PlayAt(AudioClip clip, Vector3 position, float volume = 1f)
    {
        if (clip == null) return;
        var src = GetFreeSource();
        src.transform.position = position;
        src.spatialBlend = 1f; // 3D
        src.PlayOneShot(clip, volume);
    }
}