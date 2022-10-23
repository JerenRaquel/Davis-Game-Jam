using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadRoomData : MonoBehaviour {
    public Astar astar;
    public EnemySpawner spawner;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            other.GetComponent<TargetProbeBeacon>().astar = astar;
            spawner.SwapAIState(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            spawner.SwapAIState(false);
        }
    }
}
