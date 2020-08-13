using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dungeon : MonoBehaviour
{
    [SerializeField] protected ItemDatabase itemDatabase;

    [SerializeField] protected DungeonDisplay dungeonDisplay;

    protected static RoomCoordinateEqual roomCoorEqual = 
        new RoomCoordinateEqual();
    public Dictionary<RoomCoordinate, Room> rooms {get; protected set;} = 
        new Dictionary<RoomCoordinate, Room>(roomCoorEqual);

    public List<RoomConnector> roomConnectors {get; protected set;} = 
        new List<RoomConnector>();

    public Room startRoom {get; protected set;} = new Room(0,0, startRoom : true);
    protected Room endRoom;

    private void Start() {
        
    }

    
    protected void AddRoom(Room room){
        rooms.Add(room.roomCoordinate, room);
    }

    // GetRoom is the Room that has the RoomCoordinate coordinate.
    // coordinate must be a coordinate that exists in the dungeon.
    protected Room GetRoom(RoomCoordinate coordinate){
        return rooms[coordinate];
    }

    public Room GetRoom(int x, int y){
        return rooms[new RoomCoordinate(x,y)];
    }

    public Room GetStartRoom(){
        return startRoom;
    }

}
