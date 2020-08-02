using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dungeon : MonoBehaviour
{
    private static RoomCoordinateEqual roomCoorEqual = new RoomCoordinateEqual();
    private Dictionary<RoomCoordinate, Room> rooms = new Dictionary<RoomCoordinate, Room>(roomCoorEqual);

    private Room startRoom = new Room(0,0);

    private void Awake() {
        AddRoom(startRoom);
        AddRoom(new Room(-1,0, eastRoomByDoor: startRoom));
        AddRoom(new Room(1,0, westRoomByDoor: startRoom));
        AddRoom(new Room(0,1, southRoomByDoor: startRoom));
    }

    private void AddRoom(Room room){
        rooms.Add(room.GetRoomCoordinate(), room);
    }


    // GetRoom is the Room that has the RoomCoordinate coordinate.
    // coordinate must be a coordinate that exists in the dungeon.
    public Room GetRoom(RoomCoordinate coordinate){
        return rooms[coordinate];
    }

    public Room GetStartRoom(){
        return startRoom;
    }
}
