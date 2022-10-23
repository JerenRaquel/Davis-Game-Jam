using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Astar : MonoBehaviour
{
    public enum TARGET { Player, Nearest_Door }

    public Vector2Int gridSize;
    public GameObject probe;
    public TARGET targetType = TARGET.Player;
    public bool debug = false;
    public bool isWire = false;
    public AstarProbe target = null;

    private AstarProbe[,] probes = null;
    private Vector2Int[] neighbors = {
        new Vector2Int(0, 1), new Vector2Int(0, -1),
        new Vector2Int(1, 0), new Vector2Int(-1, 0)
    };

    private void Start() {
        CreateGrid(
            new Vector2(-Mathf.FloorToInt(gridSize.x / 2) + transform.position.x,
            -Mathf.FloorToInt(gridSize.y / 2) + transform.position.y)
        );
    }

    private void OnDrawGizmos() {
        if(debug && probes != null){
            foreach(AstarProbe ap in probes) {
                ap.DrawDebug(isWire);
            }
        }
    }

    public List<Vector2> GetPath(AstarProbe origin) {
        if (origin == null) return null;
        if (probes == null) Debug.LogException(new MissingComponentException());
        ResetProbes();
        MinHeap<AstarProbe> openSet = new MinHeap<AstarProbe>(
            gridSize.x * gridSize.y,
            (AstarProbe lhs, AstarProbe rhs) => { return lhs.FCost < rhs.FCost; },
            (AstarProbe lhs, AstarProbe rhs) => { return lhs.FCost == rhs.FCost; }
        );

        Dictionary<AstarProbe, AstarProbe> cameFrom = new Dictionary<AstarProbe, AstarProbe>();
        origin.GCost = 0;
        origin.FCost = Heuristic(origin.transform.position, this.target.Index);
        openSet.Add(origin);
        while(!openSet.IsEmpty()) {
            AstarProbe current = openSet.Pop();
            if (current.Index == this.target.Index) return ReconstructPath(cameFrom, current);

            foreach(Vector2Int neighborPosition in neighbors){
                int x = neighborPosition.x + current.Index.x;
                int y = neighborPosition.y + current.Index.y;
                if (x < 0 || y < 0 || x >= probes.GetLength(0) || y >= probes.GetLength(1)) {
                    continue;
                }
                AstarProbe neighbor = probes[x, y];
                AstarProbe.PROBE_STATE neighborState = neighbor.State;
                if (neighborState == AstarProbe.PROBE_STATE.OBSTACLE) continue;
                CalculateNeighborWeight(cameFrom, openSet, current, neighbor, this.target.Index);
            }
        }
        return null;
    }

    private void CreateGrid(Vector2 start) {
        probes = new AstarProbe[gridSize.x, gridSize.y];
        for (int y = 0; y < gridSize.y; y++) {
            for (int x = 0; x < gridSize.x; x++) {
                GameObject go = Instantiate(
                    probe,
                    start + new Vector2(x, y),
                    Quaternion.identity,
                    transform
                );
                probes[x, y] = go.GetComponent<AstarProbe>();
                probes[x, y].LinkedAstar = this;
                probes[x, y].Index = new Vector2Int(x, y);
            }
        }
    }

    private void ResetProbes() {
        foreach (AstarProbe probe in this.probes) {
            probe.Reset();
        }
    }

    private List<Vector2> ReconstructPath(
        Dictionary<AstarProbe, AstarProbe> cameFrom, AstarProbe current) {
        List<Vector2> path = new List<Vector2>();
        AstarProbe c = current;
        foreach (AstarProbe node in cameFrom.Keys) {
            if(cameFrom.ContainsKey(c)){
                Vector2 coords = cameFrom[c].transform.position;
                c = cameFrom[c];
                path.Add(coords);
            }
        }
        path.Add(current.transform.position);
        return path;
    }

    private float Heuristic(Vector2 start, Vector2 end) {
        return Vector2.Distance(start, end);
    }

    private void CalculateNeighborWeight(
        Dictionary<AstarProbe, AstarProbe> cameFrom, MinHeap<AstarProbe> openSet,
        AstarProbe current, AstarProbe neighbor, Vector2Int goal) {
        float score = current.GCost + Vector2.Distance(
                current.transform.position, neighbor.transform.position
            );
        if (score < neighbor.GCost) {
            cameFrom[neighbor] = current;
            neighbor.GCost = score;
            neighbor.FCost = score + Heuristic(neighbor.transform.position, goal);
            if(!openSet.Find(neighbor)) {
                openSet.Add(neighbor);
            }
        }
    }
}
