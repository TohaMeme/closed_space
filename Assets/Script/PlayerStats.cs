using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    [Header("Oxygen")]
    public float maxOxygen = 100f;
    public float currentOxygen;

    [Tooltip("Единиц кислорода в секунду (уменьшение)")]
    public float oxygenDecreaseRate = 5f;
    [Tooltip("Интервал между тиками уменьшения (сек)")]
    public float oxygenDecreaseInterval = 1f;

    [Tooltip("Единиц восстановления в секунду (восстановление происходит плавно)")]
    public float oxygenRecoverRate = 20f;

    [Tooltip("Ссылка на компонент эффекта (OxygenEffect на Image)")]
    public OxygenEffect oxygenEffect;

    int airZoneCount = 0;
    float tickTimer = 0f;

    void Start()
    {
        currentOxygen = maxOxygen;
        if (oxygenEffect == null)
            oxygenEffect = FindObjectOfType<OxygenEffect>();
    }

    void Update()
    {
        if (airZoneCount > 0)
        {
            tickTimer += Time.deltaTime;
            while (tickTimer >= oxygenDecreaseInterval)
            {
                currentOxygen = Mathf.Max(0f, currentOxygen - oxygenDecreaseRate * oxygenDecreaseInterval);
                tickTimer -= oxygenDecreaseInterval;
            }

            float normalizedLoss = Mathf.Clamp01((maxOxygen - currentOxygen) / Mathf.Max(0.0001f, maxOxygen));
            oxygenEffect?.SetTargetIntensity(normalizedLoss);
        }
        else
        {
            if (currentOxygen < maxOxygen)
            {
                currentOxygen = Mathf.Min(maxOxygen, currentOxygen + oxygenRecoverRate * Time.deltaTime);
            }

            float normalizedLoss = Mathf.Clamp01((maxOxygen - currentOxygen) / Mathf.Max(0.0001f, maxOxygen));
            float decay = oxygenEffect != null ? oxygenEffect.quickFadeSpeed : -1f;
            oxygenEffect?.SetTargetIntensity(normalizedLoss, decay);
        }
    }

    public void EnterAirZone()
    {
        int prev = airZoneCount;
        airZoneCount++;
        if (prev == 0 && airZoneCount == 1)
        {
            float normalizedLoss = Mathf.Clamp01((maxOxygen - currentOxygen) / Mathf.Max(0.0001f, maxOxygen));
            oxygenEffect?.SetTargetIntensity(normalizedLoss);
            tickTimer = 0f;
        }
    }

    public void ExitAirZone()
    {
        airZoneCount = Mathf.Max(0, airZoneCount - 1);
        if (airZoneCount == 0)
        {
            float normalizedLoss = Mathf.Clamp01((maxOxygen - currentOxygen) / Mathf.Max(0.0001f, maxOxygen));
            float decay = oxygenEffect != null ? oxygenEffect.quickFadeSpeed : -1f;
            oxygenEffect?.SetTargetIntensity(normalizedLoss, decay);
        }
    }
}
