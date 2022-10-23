using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour {
    public Transform pivot;
    public Transform orbitObject;
    public Animator animator;
    public SwordDetector swordDetector;
    public GameObject projectilePrefab;
    public Transform shootPoint;

    private void Update() {
        Vector3 orbitVec = Camera.main.WorldToScreenPoint(pivot.position);
        orbitVec = Input.mousePosition - orbitVec;
        float angle = Mathf.Atan2(orbitVec.y, orbitVec.x) * Mathf.Rad2Deg;

        pivot.position = orbitObject.position;
        pivot.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
    }

    public void AttackEnemy(int damage) {
        if (swordDetector.enemy != null) {
            animator.SetTrigger("Swing");
            EnemyController ec = swordDetector.enemy.GetComponent<EnemyController>();
            if (ec == null) {
                ec = swordDetector.enemy.GetComponentInParent<EnemyController>();
            }
            ec.HealDamage(damage);
        }
    }

    public void Shoot(int damage) {
        Instantiate(projectilePrefab, shootPoint);
    }
}
