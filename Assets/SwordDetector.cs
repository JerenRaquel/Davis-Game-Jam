using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordDetector : MonoBehaviour {
    public GameObject enemy = null;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Enemy")) {
            enemy = other.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Enemy")) {
            enemy = null;
        }
    }
}
