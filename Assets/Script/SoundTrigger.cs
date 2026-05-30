using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundTrigger : MonoBehaviour
{

    AudioSource audioSource;
    Collider area;
    Coroutine fadeCoroutine;
    public float fadeDuration = 1.5f;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        area = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            audioSource.Play();
        }
      
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StopMusicWithFade();
        }
    }

    void StopMusicWithFade()
    {
       
        fadeCoroutine = StartCoroutine(FadeOutAndStop());
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
