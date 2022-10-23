using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

    public PathfinderController ai;
    [Header("Settings")]
    public int maxHealth;
    public float bleedDelay;
    public int bleedDamage = 2;
    public int physicalDamage;
    public float attackDelay;
    public float speed = 1f;
    public float stunDelay;
    [Header("'100 + stunRate'")]
    public float stunRate;

    public int Health { get { return currentHealth; } }
    public bool AIState { get; set; } = false;

    private int currentHealth;
    private float time;
    private float bleedTime;
    private float stunTime;
    private float moveTime;
    private bool isStunned = false;

    private void Start() {
        currentHealth = maxHealth;
    }

    private void FixedUpdate() {
        if (AIState && !isStunned && moveTime + speed <= Time.time) {
            List<Vector2> pathData = ai.FindPath();
            if (pathData == null) return;
            int index = 0; //pathData.Count - 1;
            transform.position = new Vector3(pathData[index].x, pathData[index].y, 0);
            moveTime = Time.time;
        }
        if (AIState && bleedTime + bleedDelay < Time.time) {
            bleedTime = Time.time;
            TakeDamage(bleedDamage);
        }

        if (isStunned && stunTime + stunDelay <= Time.time) {
            stunTime = Time.time;
            isStunned = false;
        }

        if (currentHealth == 0) {
            Rebirth();
        }
    }

    public void LinkAI(Astar astar) {
        ai.LinkAStar(astar);
    }

    public void TakeDamage(int damage) {
        this.currentHealth = Mathf.Clamp(currentHealth - damage, 0, maxHealth);
    }

    public void HealDamage(int damage) {
        int health = currentHealth + damage;
        int overHeal = health - maxHealth;
        if (overHeal >= stunRate) {
            isStunned = true;
        }
        this.currentHealth = health;
    }

    public void MakeStronger() {
        this.maxHealth *= 2;
        this.currentHealth = this.maxHealth;
        this.physicalDamage *= 2;
    }

    private void Rebirth() {
        MakeStronger();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            if (time + attackDelay > Time.time) return;
            time = Time.time;
            PlayerController.instance.TakeDamage(physicalDamage);
        }
    }

    private void OnTriggerStay2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            if (time + attackDelay > Time.time) return;
            time = Time.time;
            PlayerController.instance.TakeDamage(physicalDamage);
        }
    }
}
