using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    private DoorController otherSide = null;

    public Vector3 GetExitCoords() {
        Debug.Assert(this.otherSide != null);
        return this.transform.position;
    }

    public void SetExit(DoorController doorController) {
        this.otherSide = doorController;
    }

    private void OnTriggerEnter(Collider other) {
        other.transform.position = this.otherSide.GetExitCoords();
    }
}
