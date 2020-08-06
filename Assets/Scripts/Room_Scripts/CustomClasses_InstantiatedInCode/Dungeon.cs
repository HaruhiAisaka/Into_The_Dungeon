using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dungeon : MonoBehaviour
{
    private static RoomCoordinateEqual roomCoorEqual = 
        new RoomCoordinateEqual();
    [SerializeField] private RoomGenerator roomGenerator;

    private RoomGenerator currentRoomGenerator;
    private Dictionary<RoomCoordinate, Room> rooms = 
        new Dictionary<RoomCoordinate, Room>(roomCoorEqual);

    private List<RoomConnector> roomConnectors = 
        new List<RoomConnector>();

    private Room startRoom = new Room(0,0, startRoom:true);
    private Room endRoom = new Room(0,2, endRoom : true);


    private void Awake() {
        AddRoom(startRoom);
        AddRoom(new Room(-1,0));
        AddRoom(new Room(1,0));
        AddRoom(new Room(0,1));
        AddRoom(endRoom);
        roomConnectors.Add(
            new Door(GetRoom(-1,0),GetRoom(0,0),Door.DoorState.open));
        roomConnectors.Add(
            new Door(GetRoom(1,0),GetRoom(0,0),Door.DoorState.open));
        roomConnectors.Add(
            new Door(GetRoom(0,1),GetRoom(0,0),Door.DoorState.locked, 
            Door.LockedDoorColor.red)
        );
        roomConnectors.Add(
            new Stair(GetRoom(0,1),GetRoom(-1,0), 
            new Vector2(.5f,.5f), new Vector2(.5f,.5f))
        );
        roomConnectors.Add(
            new Door(GetRoom(0,1),GetRoom(0,2),Door.DoorState.open)
        );
        currentRoomGenerator = InstantiateRoom(startRoom);
        currentRoomGenerator.EnableDoorAnimations(true);
        currentRoomGenerator.EnableStairAnimations(true);
    }

    private void AddRoom(Room room){
        rooms.Add(room.roomCoordinate, room);
    }


    // GetRoom is the Room that has the RoomCoordinate coordinate.
    // coordinate must be a coordinate that exists in the dungeon.
    public Room GetRoom(RoomCoordinate coordinate){
        return rooms[coordinate];
    }

    public Room GetRoom(int x, int y){
        return rooms[new RoomCoordinate(x,y)];
    }

    public Room GetStartRoom(){
        return startRoom;
    }

    public RoomGenerator InstantiateRoom(Room room){
        RoomGenerator newRoom = Instantiate(roomGenerator);
        newRoom.GenerateRoom(room);
        currentRoomGenerator = newRoom;
        return newRoom;
    }
}
