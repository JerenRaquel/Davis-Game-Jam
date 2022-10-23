using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetProbeBeacon : MonoBehaviour {
    public Astar astar;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Probe")) {
            astar.target = other.GetComponent<AstarProbe>();
        }
    }
}
