using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    public RoomData.ROOM_TYPE type;
    public Vector2Int interiorSize;
    public GameObject wallPrefab;
    public GameObject doorPrefab;

    private int directions;

    private void Start() {
        this.directions = (int)type;
        GenerateWalls();
    }

    public void RemoveSpawnDirection(int direction) {
        this.directions ^= direction;
    }

    private void GenerateWalls() {
        Vector2 start = new Vector2(
            -Mathf.FloorToInt(interiorSize.x / 2) - 1, 
            -Mathf.FloorToInt(interiorSize.x / 2) - 1
        );
        // Top
        for(int i = 0; i < interiorSize.x + 2; i++) {
            if (i != Mathf.FloorToInt(interiorSize.x / 2) + 1 || (this.directions & 1) == 0) {
                Instantiate(wallPrefab, start + new Vector2(i, 0), Quaternion.identity, transform);
            } else {
                Instantiate(doorPrefab, start + new Vector2(i, 0), Quaternion.identity, transform);
            }
        }
        // Bottom
        for(int i = 0; i < interiorSize.x + 2; i++) {
            if (i != Mathf.FloorToInt(interiorSize.x / 2) + 1 || (this.directions & 2) == 0) {
                Instantiate(wallPrefab, start + new Vector2(i, interiorSize.y + 1), Quaternion.identity, transform);
            } else {
                Instantiate(doorPrefab, start + new Vector2(i, interiorSize.y + 1), Quaternion.identity, transform);
            }
        }
        //Side
        for(int y = 0; y < interiorSize.y; y++){
            if(y == Mathf.FloorToInt(interiorSize.y / 2) + 1){
                if((this.directions & 4) != 0) {    // Left
                    Instantiate(doorPrefab, start + new Vector2(0, y + 1), Quaternion.identity, transform);
                }
                if((this.directions & 8) != 0) {    // Right
                    Instantiate(doorPrefab, start + new Vector2(interiorSize.x + 1, y + 1), Quaternion.identity, transform);
                }
            } else {
                Instantiate(wallPrefab, start + new Vector2(0, y + 1), Quaternion.identity, transform);
                Instantiate(wallPrefab, start + new Vector2(interiorSize.x + 1, y + 1), Quaternion.identity, transform);
            }
        }
    }
}
