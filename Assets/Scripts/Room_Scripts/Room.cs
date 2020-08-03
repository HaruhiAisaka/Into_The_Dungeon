using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room
{
    public RoomCoordinate roomCoordinate {get; private set;}

    private static RoomCoordinateEqual roomCoorEqual = new RoomCoordinateEqual();

    public List<RoomConnector> roomConnectors = new List<RoomConnector>();

    // Is the doors that exist in this room.
    public Room(int x, int y)
    {
        // Sets Room Coordinate for the room.
        this.roomCoordinate = new RoomCoordinate(x,y);
    }

    public Room BlankRoom(){
        return new Room(0,0);
    }
    
    public static bool IsRoomAdjacent(Room room1, Room room2){
        Vector2 room1V = room1.roomCoordinate.GetVector2();
        Vector2 room2V = room2.roomCoordinate.GetVector2();
        float distance = Vector2.Distance(room1V,room2V);
        return (distance == 1);
    }
}
