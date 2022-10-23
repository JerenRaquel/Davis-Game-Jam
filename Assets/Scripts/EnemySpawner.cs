using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {
    public Astar astar;
    public GameObject[] enemies;
    public GameObject activeEnemy = null;
    [Header("Buttons")]
    public bool manualSpawn = false;

    private void Update() {
        if (manualSpawn) {
            manualSpawn = false;
            Spawn();
            activeEnemy.GetComponent<EnemyController>().AIState = true;
        }
    }

    public void Spawn() {
        activeEnemy = Instantiate(
            enemies[Random.Range(0, enemies.Length)],
            transform.position,
            Quaternion.identity,
            transform
        );
        activeEnemy.GetComponent<EnemyController>().LinkAI(astar);
    }

    public void SwapAIState(bool state) {
        activeEnemy.GetComponent<EnemyController>().AIState = state;
    }
}
