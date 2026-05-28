using UnityEngine;
using UnityEngine.UI;

public class OxygenEffect : MonoBehaviour
{
    [Tooltip("Image на Canvas, чей alpha будет управляться")]
    public Image image;

    [Range(0f, 1f)]
    public float maxAlpha = 0.9f;

    [Tooltip("Скорость нарастания alpha (alpha / сек)")]
    public float increaseSpeed = 0.6f;

    [Tooltip("Обычная скорость убывания alpha (alpha / сек)")]
    public float decreaseSpeed = 0.8f;

    [Tooltip("Очень быстрая скорость убывания при выходе (alpha / сек)")]
    public float quickFadeSpeed = 6f;

    float currentAlpha;
    float targetAlpha;
    float currentDecreaseSpeed;
    Color baseColor = Color.white;

    void Start()
    {
        if (image == null) image = GetComponent<Image>();
        if (image != null)
        {
            baseColor = image.color;
            SetAlpha(0f);
        }
        currentDecreaseSpeed = decreaseSpeed;
        currentAlpha = targetAlpha = 0f;
    }

    void Update()
    {
        if (image == null) return;

        float speed = (targetAlpha > currentAlpha) ? increaseSpeed : currentDecreaseSpeed;
        if (!Mathf.Approximately(currentAlpha, targetAlpha))
        {
            currentAlpha = Mathf.MoveTowards(currentAlpha, targetAlpha, speed * Time.deltaTime);
            SetAlpha(currentAlpha);
        }
    }

    void SetAlpha(float a)
    {
        if (image == null) return;
        Color c = baseColor;
        c.a = Mathf.Clamp01(a);
        image.color = c;
    }

    public void SetTargetIntensity(float normalizedLoss, float decaySpeed = -1f)
    {
        normalizedLoss = Mathf.Clamp01(normalizedLoss);
        targetAlpha = normalizedLoss * maxAlpha;
        currentDecreaseSpeed = (decaySpeed > 0f) ? decaySpeed : decreaseSpeed;
    }

    public void QuickFadeOut()
    {
        targetAlpha = 0f;
        currentDecreaseSpeed = quickFadeSpeed;
    }

    // Спрятать мгновенно
    public void HideImmediate()
    {
        targetAlpha = 0f;
        currentAlpha = 0f;
        SetAlpha(0f);
    }
}
