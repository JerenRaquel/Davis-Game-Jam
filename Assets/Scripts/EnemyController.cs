using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

    public PathfinderController ai;
    [Header("Settings")]
    public int maxHealth;
    public int physicalDamage;
    public float attackDelay;

    public int Health { get { return currentHealth; } }
    public bool AIState { get; set; } = false;

    private int currentHealth;
    private float time;

    private void FixedUpdate() {
        if (AIState) {
            List<Vector2> pathData = ai.FindPath();
            if (pathData == null) return;
            MoveTowards(pathData);
        }
    }

    public void LinkAI(Astar astar) {
        ai.LinkAStar(astar);
    }

    public void TakeDamage(int damage) {
        this.currentHealth = Mathf.Clamp(currentHealth - damage, 0, maxHealth);
    }

    private void MoveTowards(List<Vector2> pathData) {

    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            if (time + attackDelay > Time.time) return;
            time = Time.time;
            other.GetComponent<PlayerController>().TakeDamage(physicalDamage);
        }
    }
}
