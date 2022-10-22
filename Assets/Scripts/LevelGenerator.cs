using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private struct PickRoomReturnData{
        public GameObject room;
        public RoomController.ROOM_TYPE direction;
        public RoomController prev;
    }

    public uint maxRooms = 5;
    public int spacing = 8;
    public GameObject[] rooms;

    private Dictionary<RoomController.ROOM_TYPE, List<GameObject>> roomMap;
    private List<RoomController> openList;
    private List<RoomController> closedList;
    private RoomController spawnRoom = null;

    private void Start() {
        this.roomMap = new Dictionary<RoomController.ROOM_TYPE, List<GameObject>>();
        this.openList = new List<RoomController>();
        this.closedList = new List<RoomController>();
        this.roomMap.Add(RoomController.ROOM_TYPE.NORTH, new List<GameObject>());
        this.roomMap.Add(RoomController.ROOM_TYPE.SOUTH, new List<GameObject>());
        this.roomMap.Add(RoomController.ROOM_TYPE.WEST, new List<GameObject>());
        this.roomMap.Add(RoomController.ROOM_TYPE.EAST, new List<GameObject>());

        //* Convert list to dictionary for different room types -- May be unneeded if only 4 door room type
        foreach(GameObject room in this.rooms){
            RoomController rc = room.GetComponent<RoomController>();
            int roomID = (int)rc.roomType;
            // MAGIC -- Get direction from bit data
            for(int i = 0; i < 4; i++){
                if((roomID & (1 << i)) != 0){
                    // Cast from int to type -- Told you I'm doing magic; look mah I'm casting lmao, I'm tired
                    this.roomMap[(RoomController.ROOM_TYPE)(1 << i)].Add(room);
                }
            }
        }

        //Temp -- This might stay if we choose to just spawn a new scene per level instead
        Generate();
    }

    public void Generate() {
        // SpawnRoom
        this.spawnRoom = SpawnRoom(this.rooms[Random.Range(0, this.rooms.Length)], Vector2.zero);
        this.openList.Clear();
        this.openList.Add(this.spawnRoom);
        this.closedList.Add(this.spawnRoom);

        for(int i = 0; i < maxRooms; i++) {
            PickRoomReturnData chosen = PickRoom();
            // Get the room position to spawn in relative to the current room
            Vector2 position = Vector2.zero;
            switch (chosen.direction)
            {
                case RoomController.ROOM_TYPE.NORTH:
                    position = Vector2.up;
                    break;
                case RoomController.ROOM_TYPE.SOUTH:
                    position = Vector2.down;
                    break;
                case RoomController.ROOM_TYPE.WEST:
                    position = Vector2.left;
                    break;
                case RoomController.ROOM_TYPE.EAST:
                    position = Vector2.right;
                    break;
            }
            // Connect the doors
            chosen.prev.RemoveSpawnDirection((int)chosen.direction);
            RoomController rc = SpawnRoom(
                chosen.room, (position * 8) + new Vector2(chosen.prev.transform.position.x, chosen.prev.transform.position.y));
            rc.RemoveSpawnDirection((int)GetOppositeRoomDirection(chosen.direction));
            // Add room to rooms that need to be connected
            this.openList.Add(rc);
            // If the room is fully surrounded, remove from the rooms attempting to connect
            if(chosen.prev.isSurrounded) this.openList.Remove(chosen.prev);
            // Used for sealing
            this.closedList.Add(rc);
        }

        // Remove doors that don't connect to any room
        foreach(RoomController rc in this.closedList) {
            rc.SealFakeDoors();
        }
    }

    private PickRoomReturnData PickRoom() {
        // Choose a room wanting to connect
        RoomController chosen = this.openList[Random.Range(0, this.openList.Count)];
        // Choose a spawning direction relative to chosen room
        int openDoors = chosen.OpenDoors;
        RoomController.ROOM_TYPE[] directions = ParseDirections(openDoors);
        RoomController.ROOM_TYPE chosenDirection = directions[Random.Range(0, directions.Length)];
        int count = this.roomMap[chosenDirection].Count;
        // Pack data and return results
        PickRoomReturnData data;
        data.room = this.roomMap[chosenDirection][Random.Range(0, count)];
        data.direction = chosenDirection;
        data.prev = chosen;
        return data;
    }

    private RoomController SpawnRoom(GameObject room, Vector2 position) {
        GameObject go =  Instantiate(room, position, Quaternion.identity, transform);
        return go.GetComponent<RoomController>();
    }

    private RoomController.ROOM_TYPE[] ParseDirections(int packedData) {
        // MAGIC! -- convert packed int bits to array of direction choices
        List<RoomController.ROOM_TYPE> directions = new List<RoomController.ROOM_TYPE>();
        for(int i = 0; i < 4; i++){
            if((packedData & (1 << i)) != 0){ 
                directions.Add((RoomController.ROOM_TYPE)(packedData & (1 << i)));
            }
        }
        RoomController.ROOM_TYPE[] result = directions.ToArray();
        return result;
    }

    private RoomController.ROOM_TYPE GetOppositeRoomDirection(RoomController.ROOM_TYPE type) {
        if (type == RoomController.ROOM_TYPE.NORTH) return RoomController.ROOM_TYPE.SOUTH;
        else if (type == RoomController.ROOM_TYPE.SOUTH) return RoomController.ROOM_TYPE.NORTH;
        else if (type == RoomController.ROOM_TYPE.WEST) return RoomController.ROOM_TYPE.EAST;
        else return RoomController.ROOM_TYPE.WEST;
    }
}
