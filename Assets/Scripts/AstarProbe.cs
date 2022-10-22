using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AstarProbe : MonoBehaviour
{
    public enum PROBE_STATE { EMPTY, TARGET, OBSTACLE }

    public Astar LinkedAstar { get; set;} = null;
    public PROBE_STATE State { get; set; } = PROBE_STATE.EMPTY;

    public void DrawDebug(){
        var prevColor = Gizmos.color;
        if(State == PROBE_STATE.EMPTY){
            Gizmos.color = Color.blue;
        } else if(State == PROBE_STATE.TARGET) {
            Gizmos.color = Color.yellow;
        } else {
            Gizmos.color = Color.red;
        }
        Gizmos.DrawCube(transform.position, new Vector3(1, 1, 0));
        Gizmos.color = prevColor;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag(Enum.GetName(typeof(Astar.TARGET), LinkedAstar.goal))){
            State = PROBE_STATE.TARGET;
        } else if(other.CompareTag("Obstacle")){
            State = PROBE_STATE.OBSTACLE;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        State = PROBE_STATE.EMPTY;
    }
}
