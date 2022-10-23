using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    public enum ROOM_TYPE { 
        NORTH = 0b0000_0001,    // 1
        SOUTH = 0b0000_0010,    // 2
        WEST = 0b0000_0100,     // 4
        EAST = 0b0000_1000,     // 8
        STRAIGHT_VERTICAL = NORTH | SOUTH,
        STRAIGHT_HORIZONTAL = WEST | EAST,
        NORTH_EAST = NORTH | EAST,
        NORTH_WEST = NORTH | WEST,
        SOUTH_EAST = SOUTH | EAST,
        SOUTH_WEST = SOUTH | WEST,
        All = STRAIGHT_HORIZONTAL | STRAIGHT_VERTICAL
    }

    public ROOM_TYPE roomType;
    public GameObject wallPrefab;
    public DoorController north;
    public DoorController south;
    public DoorController west;
    public DoorController east;

    private int directions;
    private int connectedDoors;

    private void Awake() {
        this.directions = (int)roomType;
    }

    public void RemoveSpawnDirection(int direction) {
        this.directions ^= direction;
        connectedDoors |= direction;
    }

    public void SealFakeDoors() {
        if((this.connectedDoors & (int)ROOM_TYPE.NORTH) == 0) {
            ReplaceDoorWithWall(north);
        }
        if((this.connectedDoors & (int)ROOM_TYPE.SOUTH) == 0) {
            ReplaceDoorWithWall(south);
        }
        if((this.connectedDoors & (int)ROOM_TYPE.WEST) == 0) {
            ReplaceDoorWithWall(west);
        }
        if((this.connectedDoors & (int)ROOM_TYPE.EAST) == 0) {
            ReplaceDoorWithWall(east);
        }
    }

    public int OpenDoors {
        get { return this.directions; }
    }

    public bool isSurrounded {
        get { return this.directions == 0; }
    }

    private void ReplaceDoorWithWall(DoorController dc) {
        if (dc == null) return;
        GameObject door = dc.gameObject;
        Vector3 position = door.transform.position;
        Instantiate(wallPrefab, position, Quaternion.identity, transform);
    }
}
