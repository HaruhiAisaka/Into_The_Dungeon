using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dungeon : MonoBehaviour
{
    [SerializeField] protected ItemDatabase itemDatabase;

    [SerializeField] protected DungeonDisplay dungeonDisplay;


    [SerializeField] protected int squareArea = 10;

    [SerializeField] protected int minRooms = 20;
    [SerializeField] protected int maxRooms = 20;

    [SerializeField] protected int minClusters = 1;
    [SerializeField] protected int maxClusters = 1;

    private static List<String> enemies = new List<string>(){"skull", "blob", "bat", "shooter"};


    // This float is used to calculate a range of values of the number 
    // of rooms that can be made in a given cluster. 
    // This is done so that one cluster can have more rooms connected 
    // to it compared to another.
    // REQUIRES the float be <1.0
    [SerializeField] protected float roomsInClusterRandomnessFactor = .2f;

    protected System.Random rand;

    public List<Room> rooms { get; protected set; } = new List<Room>();

    public List<RoomConnector> roomConnectors { get; protected set; } =
        new List<RoomConnector>();
 
    public Room startRoom { get; protected set; } = new Room(0, 0, startRoom: true, enemies : new List<string>(){"skull"});
    protected Room endRoom;

    private void Start() 
    {
        rand = new System.Random();
        rooms.Add(startRoom);
        GenerateDungeon();
        dungeonDisplay.CreateDungeon(this);
    }

    private void GenerateDungeon()
    {
        int numOfRoomsLeft = rand.Next(minRooms, maxRooms);
        int numOfClusters = rand.Next(minClusters, maxClusters + 1);
        int numOfRoomsForNextCluster;
        for (int numOfClustersLeft = numOfClusters; numOfClustersLeft >= 1; numOfClustersLeft--)
        {
            Debug.Log("Numer of Rooms Left: " + numOfRoomsLeft);
            if (numOfClustersLeft == 1)
            {
                numOfRoomsForNextCluster = numOfRoomsLeft;
            }
            else
            {
                numOfRoomsForNextCluster = rand.Next(
                (int)((numOfRoomsLeft / numOfClustersLeft) * (1 - roomsInClusterRandomnessFactor)),
                (int)((numOfRoomsLeft / numOfClustersLeft) * (1 + roomsInClusterRandomnessFactor)));
            }
            numOfRoomsLeft -= numOfRoomsForNextCluster;
            Debug.Log("Numer of Rooms For Next Cluster: " + numOfRoomsForNextCluster);
            Debug.Log("Numer of Rooms Left: " + numOfRoomsLeft);
            if (numOfClustersLeft == numOfClusters)
            {
                numOfRoomsForNextCluster--;
                GenerateCluster(startRoom, numOfRoomsForNextCluster);
            }
            else
            {
                Room newRoom = GenerateRandomRoom();
                rooms.Add(newRoom);
                numOfRoomsForNextCluster--;
                GenerateCluster(newRoom, numOfRoomsForNextCluster);
            }

        }


    }



    // GenerateCluster() Generates all the rooms that are in the dungeon 
    // in accordance to the properties of all dungeons.
    // Dungeon Properties:
    // 1. Room(0,-1) can not be generated (this is used for an animation in the beginning of the game).
    // 2. Rooms must fit in the area of the dungeon as defined by the Rect object in this class.
    // 3. A cluster is a group of rooms that are adjecent to eachother, only three clusters can exist in a dungeon.
    // Currently, only one cluster is made.
    private void GenerateCluster(Room room, int numOfRooms)
    {
        List<RoomCoordinate> possibleRooms = new List<RoomCoordinate>();
        RoomCoordinate newestRoom = room.roomCoordinate;
        for (int i = 0; i < numOfRooms; i++)
        {
            // Adds new possible surrounding coordinates to the pool of possible coordinates
            // Possible coordinates must be adjecent to a currently made room. 
            RoomCoordinate[] surroundingRCs = SurroundingsRoomCoordinates(newestRoom);
            foreach (RoomCoordinate surroundingRC in surroundingRCs)
            {
                if (!RoomExists(surroundingRC) &&
                !possibleRooms.Exists(rC => rC.Equals(surroundingRC)) &&
                RCInDungeonBorders(surroundingRC))
                {
                    possibleRooms.Add(surroundingRC);
                }
            }
            // Picks one coordinate from the pool of possible ones
            int index = rand.Next(possibleRooms.Count);
            // Creates new room from that coordinates
            Room newRoom = new Room(possibleRooms[index]);
            // Removes the used coordinate from the pool
            possibleRooms.Remove(possibleRooms[index]);
            // Updates the variable that holds the newest room
            newestRoom = newRoom.roomCoordinate;
            // Adds the room to the list of existing rooms
            rooms.Add(newRoom);
            // Finds a room that is adjecent to the newly created room
            Room adjacentRoom = GetRandomAdjacentRoom(newRoom);
            // Connects those two rooms with a door.
            roomConnectors.Add(new Door(adjacentRoom, newRoom, Door.DoorState.open));
        }
    }

    private Room GenerateRandomRoom()
    {
        bool dublicateRoomMade;
        RoomCoordinate newRC;
        do
        {
            int x = rand.Next(-squareArea / 2, squareArea / 2 + 1);
            int y = rand.Next(0, squareArea + 1);
            newRC = new RoomCoordinate(x, y);
            dublicateRoomMade = RoomExists(newRC);
        } while (dublicateRoomMade);
        return new Room(newRC);
    }

    private Room RandomRoomFromExistingRooms()
    {
        int index = rand.Next(rooms.Count);
        return rooms[index];
    }

    private RoomCoordinate[] SurroundingsRoomCoordinates(RoomCoordinate roomCoordinate)
    {
        int x = roomCoordinate.x;
        int y = roomCoordinate.y;
        RoomCoordinate[] result =
            {new RoomCoordinate(x-1,y),
            new RoomCoordinate(x+1,y),
            new RoomCoordinate(x,y-1),
            new RoomCoordinate(x,y+1)};
        return result;
    }

    private bool RCInDungeonBorders(RoomCoordinate roomCoordinate)
    {
        int x = roomCoordinate.x;
        int y = roomCoordinate.y;
        bool inX = (x >= (-squareArea / 2) && x <= (squareArea / 2));
        bool inY = (y >= 0 && y <= squareArea);
        return inX && inY;
    }

    private Room GetRandomAdjacentRoom(Room room)
    {
        RoomCoordinate[] newSurroundingRCs =
            SurroundingsRoomCoordinates(room.roomCoordinate);
        List<RoomCoordinate> adjacentRooms = new List<RoomCoordinate>();
        foreach (RoomCoordinate RC in newSurroundingRCs)
        {
            if (rooms.Exists(aRoom => aRoom.roomCoordinate.Equals(RC)))
            {
                adjacentRooms.Add(RC);
            }
        }
        int index = rand.Next(adjacentRooms.Count);
        return GetRoom(adjacentRooms[index]);
    }

    protected void AddRoom(Room room)
    {
        rooms.Add(room);
    }

    // GetRoom is the Room that has the RoomCoordinate coordinate.
    // coordinate must be a coordinate that exists in the dungeon.
    protected Room GetRoom(RoomCoordinate coordinate)
    {
        return rooms.Find(room => room.roomCoordinate.Equals(coordinate));
    }

    public Room GetRoom(int x, int y)
    {
        return GetRoom(new RoomCoordinate(x, y));
    }

    protected bool RoomExists(RoomCoordinate coordinate)
    {
        return rooms.Exists(room => room.roomCoordinate.Equals(coordinate));
    }

    public Room GetStartRoom()
    {
        return startRoom;
    }

}
