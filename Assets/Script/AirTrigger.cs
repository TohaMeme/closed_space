using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirTrigger : MonoBehaviour
{
    [Tooltip("¬ключить дл€ отладки входа/выхода из зоны")]
    public bool debug = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        PlayerStats ps = FindPlayerStats(other);
        if (ps != null)
        {
            ps.EnterAirZone();
            if (debug) Debug.Log($"[AirTrigger] Enter -> {ps.gameObject.name}");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        PlayerStats ps = FindPlayerStats(other);
        if (ps != null)
        {
            ps.ExitAirZone();
            if (debug) Debug.Log($"[AirTrigger] Exit -> {ps.gameObject.name}");
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
