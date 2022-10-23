using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AstarProbe : MonoBehaviour
{
    public enum PROBE_STATE { EMPTY, TARGET, OBSTACLE }

    public Astar LinkedAstar { get; set;} = null;
    public PROBE_STATE State { get; set; } = PROBE_STATE.EMPTY;
    public float GCost { get; set; } = float.MaxValue;
    public float FCost { get; set; } = float.MaxValue;
    public Vector2Int Index { get; set; }

    public void DrawDebug(bool isWire) {
        var prevColor = Gizmos.color;
        if(State == PROBE_STATE.EMPTY){
            Gizmos.color = Color.blue;
        } else if(State == PROBE_STATE.TARGET) {
            Gizmos.color = Color.yellow;
        } else {
            Gizmos.color = Color.red;
        }
        if (isWire) {
            Gizmos.DrawWireCube(transform.position, new Vector3(1, 1, 0));
        } else {
            Gizmos.DrawCube(transform.position, new Vector3(1, 1, 0));
        }
        Gizmos.color = prevColor;
    }

    public void Reset() {
        GCost = float.MaxValue;
        FCost = float.MaxValue;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag(Enum.GetName(typeof(Astar.TARGET), LinkedAstar.targetType))) {
            State = PROBE_STATE.TARGET;
        } else if(other.CompareTag("Obstacle")){
            State = PROBE_STATE.OBSTACLE;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        State = PROBE_STATE.EMPTY;
    }
}
