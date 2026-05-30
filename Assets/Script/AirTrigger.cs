using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirTrigger : MonoBehaviour
{
    [Tooltip("ќтвечает за включение музыки/вывод в лог")]
    public bool debug = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        // ”величиваем счетчик зон в AmbientMusicManager Ч музыка не прервЄтс€ при переходе между триггерами
        AmbientMusicManager.Instance.EnterZone();

        PlayerStats ps = FindPlayerStats(other);
        if (ps != null)
        {
            ps.EnterAirZone();
            if (debug) Debug.Log($"[AirTrigger] Enter -> {ps.gameObject.name}");
        }
        else
        {
            if (debug) Debug.Log("[AirTrigger] Enter -> PlayerStats not found, AmbientMusicManager used directly");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        // ”меньшаем счетчик зон Ч музыка затухнет лишь когда счетчик станет 0
        AmbientMusicManager.Instance.ExitZone();

        PlayerStats ps = FindPlayerStats(other);
        if (ps != null)
        {
            ps.ExitAirZone();
            if (debug) Debug.Log($"[AirTrigger] Exit -> {ps.gameObject.name}");
        }
        else
        {
            if (debug) Debug.Log("[AirTrigger] Exit -> PlayerStats not found, AmbientMusicManager used directly");
        }
    }

    private PlayerStats FindPlayerStats(Collider other)
    {
        if (other.attachedRigidbody != null)
        {
            var ps = other.attachedRigidbody.GetComponent<PlayerStats>();
            if (ps != null) return ps;
        }

        var psParent = other.GetComponentInParent<PlayerStats>();
        if (psParent != null) return psParent;

        return other.transform.root.GetComponent<PlayerStats>();
    }
}
