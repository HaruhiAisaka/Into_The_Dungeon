using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room
{
    public RoomCoordinate roomCoordinate {get; private set;}

    private static RoomCoordinateEqual roomCoorEqual = new RoomCoordinateEqual();

    public List<RoomConnector> roomConnectors {get; private set;} = new List<RoomConnector>();
    public List<Door> doors {get; private set;} = new List<Door>();
    public List<Stair> stairs {get; private set;} = new List<Stair>();
    public List<Item> items {get; private set;} = new List<Item>();

    public bool startRoom = false;
    public bool endRoom = false;


    // Dimentions of a room.
    public const int LENGTH_FROM_CENTER_X = 7;
    public const int LENGTH_FROM_CENTER_Y = 3;

    // Is the doors that exist in this room.
    public Room(int x, int y, bool startRoom = false, bool endRoom = false)
    {
        // Sets Room Coordinate for the room.
        this.roomCoordinate = new RoomCoordinate(x,y);
        this.startRoom = startRoom;
        this.endRoom = endRoom;
        if (startRoom){
            Room dummyRoom = new Room(0,-1);
            Door dummyDoor = new Door(dummyRoom, this, Door.DoorState.open);
        }
    }

    public Room(RoomCoordinate roomCoordinate, bool startRoom = false, bool endRoom = false){
        // Sets Room Coordinate for the room.
        this.roomCoordinate = roomCoordinate;
        this.startRoom = startRoom;
        this.endRoom = endRoom;
        if (startRoom){
            Room dummyRoom = new Room(0,-1);
            Door dummyDoor = new Door(dummyRoom, this, Door.DoorState.open);
        }
    }

    public void AddRoomConnector(RoomConnector roomConnector){  
        roomConnectors.Add(roomConnector);
        if (roomConnector is Door){
            doors.Add((Door) roomConnector);
        }
        else if(roomConnector is Stair){
            stairs.Add((Stair) roomConnector);
        }
    }

    public void AddItem(Item item, Vector2 localPosition){
        item.SetItemPosition(localPosition);
        items.Add(item);
    }
    
    public static bool IsRoomAdjacent(Room room1, Room room2){
        Vector2 room1V = room1.roomCoordinate.GetVector2();
        Vector2 room2V = room2.roomCoordinate.GetVector2();
        float distance = Vector2.Distance(room1V,room2V);
        return (distance == 1);
    }


    #region IsInRoom Methods

    public static bool IsInRoom(GameObject gameObject){
        float x = gameObject.transform.position.x;
        float y = gameObject.transform.position.y;
        float xSize = gameObject.transform.localScale.x;
        float ySize = gameObject.transform.localScale.y;
        bool inX = 
            (x - xSize >= -LENGTH_FROM_CENTER_X && 
            x + xSize <= LENGTH_FROM_CENTER_X);
        bool inY = 
            (y - ySize >= -LENGTH_FROM_CENTER_Y &&
            y + ySize <= LENGTH_FROM_CENTER_Y);
        return (inX && inY);
    }

    public static bool IsInRoom(float x, float y, float xSize, float ySize){
        bool inX = 
            (x - xSize >= -LENGTH_FROM_CENTER_X && 
            x + xSize <= LENGTH_FROM_CENTER_X);
        bool inY = 
            (y - ySize >= -LENGTH_FROM_CENTER_Y &&
            y + ySize <= LENGTH_FROM_CENTER_Y);
        return (inX && inY);
    }

    public static bool IsInRoom(Vector2 position, float xSize, float ySize){
        bool inX = 
            (position.x - xSize >= -LENGTH_FROM_CENTER_X && 
            position.x + xSize <= LENGTH_FROM_CENTER_X);
        bool inY = 
            (position.y - ySize >= -LENGTH_FROM_CENTER_Y &&
            position.y + ySize <= LENGTH_FROM_CENTER_Y);
        return (inX && inY);
    }

    #endregion
}
