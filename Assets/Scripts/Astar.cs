using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Astar : MonoBehaviour
{
    public enum TARGET { Player, Nearest_Door }

    private struct Node {
        public float fscore;
        public float gscore;
        public Vector2 position;
    }

    public Vector2Int gridSize;
    public GameObject probe;
    public TARGET goal = TARGET.Player;
    public bool debug = false;

    private AstarProbe[,] probes = null;
    private Vector2Int[] neighbors = {
        new Vector2Int(0, 1), new Vector2Int(0, -1),
        new Vector2Int(1, 0), new Vector2Int(-1, 0)
    };

    private void Start() {
        CreateGrid(new Vector2(-Mathf.FloorToInt(gridSize.x / 2), -Mathf.FloorToInt(gridSize.y / 2)));
    }

    private void OnDrawGizmos() {
        if(debug && probes != null){
            foreach(AstarProbe ap in probes) {
                ap.DrawDebug();
            }
        }
    }

    private void CreateGrid(Vector2 start) {
        probes = new AstarProbe[gridSize.x, gridSize.y];
        for(int y = 0; y < gridSize.y; y++){
            for(int x = 0; x < gridSize.x; x++){
                GameObject go = Instantiate(probe, start + new Vector2(x, y), Quaternion.identity, transform);
                go.GetComponent<AstarProbe>().LinkedAstar = this;
                probes[x,y] = go.GetComponent<AstarProbe>();
            }
        }
    }

    public List<Vector2> GetPath(Vector2 origin, Vector2 goal) {
        MinHeap<Node> openSet = new MinHeap<Node>(
            gridSize.x * gridSize.y, 
            (Node a, Node b) => { return a.fscore < b.fscore; }, 
            (Node a, Node b) => { return a.fscore == b.fscore; }
        );

        Dictionary<Node, Node> cameFrom = new Dictionary<Node, Node>();
        Node start;
        start.gscore = 0;
        start.fscore = Heuristic(origin, goal);
        start.position = origin;
        openSet.Add(start);

        while(!openSet.IsEmpty()) {
            Node current = openSet.Pop();
            if(current.position == goal) return ReconstructPath(cameFrom, current);

            foreach(Vector2Int neighborPosition in neighbors){
                AstarProbe.PROBE_STATE neighborState = 
                    probes[neighborPosition.x + (int)current.position.x, neighborPosition.y + (int)current.position.y].State;
                if(neighborState == AstarProbe.PROBE_STATE.OBSTACLE) continue;
                Node neighbor;
                neighbor.gscore = float.MaxValue;
                neighbor.fscore = float.MaxValue;
                neighbor.position = new Vector2(neighborPosition.x + (int)current.position.x, neighborPosition.y + (int)current.position.y);
                CalculateNeighborWeight(cameFrom, openSet, current, neighbor, goal);
            }
        }
        Debug.LogAssertion("AStar FAILURE!");
        return null;
    }

    private List<Vector2> ReconstructPath(Dictionary<Node, Node> cameFrom, Node current) {
        List<Vector2> path = new List<Vector2>();
        Node c = current;
        foreach(Node node in cameFrom.Keys){
            if(cameFrom.ContainsKey(c)){
                Vector2 coords = cameFrom[c].position;
                c = cameFrom[c];
                path.Add(coords);
            }
        }
        return path;
    }

    private float Heuristic(Vector2 start, Vector2 end) {
        return Mathf.Abs(start.x - end.x) + Mathf.Abs(start.y - end.y);
    }

    private void CalculateNeighborWeight(
        Dictionary<Node, Node> cameFrom, MinHeap<Node> openSet, 
        Node current, Node neighbor, Vector2 goal ){
        float potentialGScore = current.gscore + Vector2.Distance(current.position, neighbor.position);
        if(potentialGScore < neighbor.gscore) {
            cameFrom[neighbor] = current;
            neighbor.gscore = potentialGScore;
            neighbor.fscore = potentialGScore + Heuristic(neighbor.position, goal);
            if(!openSet.Find(neighbor)) {
                openSet.Add(neighbor);
            }
        }
    }
}
