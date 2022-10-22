using UnityEngine;

[CreateAssetMenu(fileName = "RoomData", menuName = "Davis Game Jam 2022/RoomData", order = 0)]
public class RoomData : ScriptableObject {
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

    public GameObject prefab;
    public ROOM_TYPE roomType;
}