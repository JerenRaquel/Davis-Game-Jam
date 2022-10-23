using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfinderController : MonoBehaviour {
    [Header("Settings")]
    public Gradient lineColors;
    public Vector2 lineWidth;
    [Header("Buttons")]
    public bool generate = false;
    public bool debug = false;
    public bool debugDynamic = false;

    private Astar astar;
    private Vector3 pastLocation;
    private AstarProbe currentProbePosition;

    // Line Visual
    private GameObject line;
    private LineRenderer lineRenderer;

    private void Start() {
        line = new GameObject();
        lineRenderer = line.AddComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.colorGradient = lineColors;
        lineRenderer.startWidth = lineWidth.x;
        lineRenderer.endWidth = lineWidth.y;
        line.SetActive(false);
    }

    private void Update() {
        if (generate) {
            generate = false;
            // Do something
            List<Vector2> pathData = astar.GetPath(currentProbePosition);
        } else if (debug || debugDynamic) {
            debug = false;
            CreateLinePath();
        }
    }

    public List<Vector2> FindPath() {
        if (astar == null) return null;
        return astar.GetPath(currentProbePosition);
    }

    public void LinkAStar(Astar astar) {
        this.astar = astar;
    }

    private void CreateLinePath() {
        line.SetActive(false);
        List<Vector2> pathData = astar.GetPath(currentProbePosition);
        if (pathData == null) return;

        lineRenderer.positionCount = pathData.Count;
        for (int i = 0; i < pathData.Count; i++) {
            lineRenderer.SetPosition(i, pathData[pathData.Count - i - 1]);
        }
        line.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Probe")) {
            this.currentProbePosition = other.GetComponent<AstarProbe>();
        }
    }

    private void OnTriggerStay2D(Collider2D other) {
        if (other.CompareTag("Probe")) {
            this.currentProbePosition = other.GetComponent<AstarProbe>();
        }
    }
}
