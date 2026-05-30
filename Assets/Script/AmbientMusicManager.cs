using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class AmbientMusicManager : MonoBehaviour
{
    static AmbientMusicManager _instance;
    public static AmbientMusicManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<AmbientMusicManager>();
                if (_instance == null)
                {
                    var go = new GameObject("AmbientMusicManager");
                    _instance = go.AddComponent<AmbientMusicManager>();
                }
            }
            return _instance;
        }
    }

    [Tooltip("Аудиоклип для зон")]
    public AudioClip ambientClip;
    [Tooltip("Громкость при полной громкости")]
    public float targetVolume = 1f;
    [Tooltip("Длительность плавного затухания/появления в секундах")]
    public float fadeDuration = 1.5f;

    AudioSource audioSource;
    int zoneCount = 0;
    Coroutine fadeCoroutine;

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.playOnAwake = false;
        audioSource.loop = true;
        if (ambientClip != null && audioSource.clip == null) audioSource.clip = ambientClip;
        audioSource.volume = 0f;
    }

    public void EnterZone()
    {
        zoneCount = Mathf.Max(0, zoneCount) + 1;
        if (zoneCount == 1)
        {
            StartMusic();
        }
    }

    public void ExitZone()
    {
        zoneCount = Mathf.Max(0, zoneCount - 1);
        if (zoneCount == 0)
        {
            StopMusicWithFade();
        }
    }

    void StartMusic()
    {
        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        if (audioSource.clip == null && ambientClip != null) audioSource.clip = ambientClip;
        if (!audioSource.isPlaying) audioSource.Play();
        fadeCoroutine = StartCoroutine(FadeTo(targetVolume));
    }

    void StopMusicWithFade()
    {
        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        fadeCoroutine = StartCoroutine(FadeOutAndStop());
    }

    IEnumerator FadeTo(float target)
    {
        float start = audioSource.volume;
        float t = 0f;
        if (fadeDuration <= 0f)
        {
            audioSource.volume = target;
            yield break;
        }

        while (t < fadeDuration)
        {
            t += Time.unscaledDeltaTime;
            audioSource.volume = Mathf.Lerp(start, target, t / fadeDuration);
            yield return null;
        }
        audioSource.volume = target;
        fadeCoroutine = null;
    }

    IEnumerator FadeOutAndStop()
    {
        float start = audioSource.volume;
        float t = 0f;
        if (fadeDuration <= 0f)
        {
            audioSource.volume = 0f;
            audioSource.Stop();
            yield break;
        }

        while (t < fadeDuration)
        {
            t += Time.unscaledDeltaTime;
            audioSource.volume = Mathf.Lerp(start, 0f, t / fadeDuration);
            yield return null;
        }
        audioSource.volume = 0f;
        audioSource.Stop();
        fadeCoroutine = null;
    }
}