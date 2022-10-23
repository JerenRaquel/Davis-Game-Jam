using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadRoomData : MonoBehaviour {
    public Astar astar;
    public EnemySpawner spawner;

    private bool isSpawned = false;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            PlayerController.instance.targetProbe.astar = astar;
            if (!isSpawned) {
                //! ITS NOW A FEATURE!
                // isSpawned = true;
                spawner.Spawn();
                spawner.SwapAIState(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            spawner.SwapAIState(false);
        }
    }
}
