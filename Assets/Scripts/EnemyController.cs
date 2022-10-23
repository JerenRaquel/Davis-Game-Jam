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

    public int Health { get { return currentHealth; } }
    public bool AIState { get; set; } = false;

    private int currentHealth;
    private float time;
    private float bleedTime;
    private bool isMoving = false;

    private void Start() {
        currentHealth = maxHealth;
    }

    private void FixedUpdate() {
        if (AIState && !isMoving) {
            List<Vector2> pathData = ai.FindPath();
            if (pathData == null) return;
            isMoving = true;
            StartCoroutine(MoveTowards(pathData));
        }
        if (AIState && bleedTime + bleedDelay < Time.time) {
            bleedTime = Time.time;
            TakeDamage(bleedDamage);
        }

        if (currentHealth == 0) {
            Debug.Log("DEADGE");
            Destroy(this.gameObject);
        }
    }

    public void LinkAI(Astar astar) {
        ai.LinkAStar(astar);
    }

    public void TakeDamage(int damage) {
        this.currentHealth = Mathf.Clamp(currentHealth - damage, 0, maxHealth);
    }

    public void HealDamage(int damage) {
        this.currentHealth = Mathf.Clamp(currentHealth + damage, 0, maxHealth);
    }

    private IEnumerator MoveTowards(List<Vector2> pathData) {
        transform.position = new Vector3(pathData[0].x, pathData[0].y, 0);
        yield return new WaitForSeconds(speed);
        isMoving = false;
        yield return new WaitForEndOfFrame();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            if (time + attackDelay > Time.time) return;
            time = Time.time;
            other.GetComponent<PlayerController>().TakeDamage(physicalDamage);
        }
    }
}
