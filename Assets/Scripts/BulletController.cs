using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour {
    public float lifeTime;
    public float speed;
    public Rigidbody2D rb;

    [HideInInspector] public int damage;

    private void Start() {
        Destroy(this.gameObject, lifeTime);
        rb.AddRelativeForce(transform.up * speed);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Enemy")) {
            EnemyController ec = other.GetComponent<EnemyController>();
            if (ec == null) {
                ec = other.GetComponentInParent<EnemyController>();
            }
            ec.TakeDamage(damage);
            Destroy(this.gameObject);
        }
    }
}
