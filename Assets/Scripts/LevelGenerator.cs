using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LevelGenerator : MonoBehaviour
{
    #region Class Instance
    public static LevelGenerator instance = null;
    private void CreateInstance()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
        }
    #endregion
    private void Awake() => CreateInstance();

    public uint maxRooms = 5;
    public RoomData[] rooms;

    private Dictionary<RoomData.ROOM_TYPE, List<RoomData>> roomMap;

    private void Start() {
        this.roomMap = new Dictionary<RoomData.ROOM_TYPE, List<RoomData>>();
        this.roomMap.Add(RoomData.ROOM_TYPE.NORTH, new List<RoomData>());
        this.roomMap.Add(RoomData.ROOM_TYPE.SOUTH, new List<RoomData>());
        this.roomMap.Add(RoomData.ROOM_TYPE.WEST, new List<RoomData>());
        this.roomMap.Add(RoomData.ROOM_TYPE.EAST, new List<RoomData>());

        foreach(RoomData room in this.rooms){
            int roomID = (int)room.roomType;
            if((roomID & 1) != 0) {
                this.roomMap[RoomData.ROOM_TYPE.NORTH].Add(room);
            } else if ((roomID & 2) != 0) {
                this.roomMap[RoomData.ROOM_TYPE.SOUTH].Add(room);
            } else if ((roomID & 4) != 0) {
                this.roomMap[RoomData.ROOM_TYPE.WEST].Add(room);
            } else {
                this.roomMap[RoomData.ROOM_TYPE.EAST].Add(room);
            }
        }
    }

    public void Generate() {
        RoomData choosen = PickRoom();
        SpawnRoom(choosen, Vector2.up);
    }

    private RoomData PickRoom() {
        return null;
    }

    private void SpawnRoom(RoomData room, Vector2 position) {

    }
}
