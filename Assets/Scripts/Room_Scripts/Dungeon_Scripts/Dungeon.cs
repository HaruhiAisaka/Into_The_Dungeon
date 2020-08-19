using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dungeon : MonoBehaviour
{
    [SerializeField] protected ItemDatabase itemDatabase;

    [SerializeField] protected DungeonDisplay dungeonDisplay;


    [SerializeField] protected int squareArea = 10;

    [Header("Procedural Generation Parameters")]
    [SerializeField] [Range(1,30)] protected int minRoomsPerCluster = 20;
    [SerializeField] [Range(1,30)] protected int maxRoomsPerCluster = 20;

    [SerializeField] [Range(2,10)] protected int minClusters = 1;
    [SerializeField] [Range(2,10)] protected int maxClusters = 1;

    [SerializeField] [Range(0,1)] protected float stairDoorRatio = .2f;

    private static List<String> enemies = new List<string>(){"skull", "blob", "bat", "shooter"};

    public System.Random rand {get; private set;}

    public Dictionary<RoomCoordinate, Room> rooms {get; private set;} = 
        new Dictionary<RoomCoordinate, Room>(new RoomCoordinateEqual());

    public List<RoomConnector> roomConnectors { get; protected set; } =
        new List<RoomConnector>();
 
    public Room startRoom { get; protected set;}
    protected Room finalBossRoom;
    protected Room endRoom;


    // A cluster is a series of rooms all connected by open doors.
    // Meaning that the player can enter into all the rooms without the need of keys to open locked doors.
    // This distinction is used to simplify and further abstract the procedural generation algorithm.
    public class Cluster{
        private Dungeon dungeon;
        public Dictionary<RoomCoordinate, Room> rooms {get; private set;} = 
            new Dictionary<RoomCoordinate, Room>(new RoomCoordinateEqual());
        public int clusterNum {get; private set;}
        private static int nextClusterNum = 0;

        // Room adjacentRoom is a room outside of the current cluster that is adjacent to the cluster. 
        // This information is used when two clusters are ment to be connected by door.
        public List<(Cluster,List<(Room,Room)>)> adjacentClusters {get; private set;} =
            new List<(Cluster, List<(Room, Room)>)>();
        public Cluster(Dungeon dungeon, Room startingRoom){
            this.dungeon = dungeon;
            this.clusterNum = nextClusterNum;
            nextClusterNum ++;
            dungeon.clusters.Add(this);
            // If the room doesn't already exist in the dungeon, add it.
            if (!dungeon.RoomExists(startingRoom)){
                dungeon.AddRoom(startingRoom);
            }
            startingRoom.cluster = this.clusterNum;
            rooms[startingRoom.roomCoordinate] = startingRoom;
            UpdateAdjacentRooms(startingRoom);
        }

        public void AddRoom(Room room, Room connectingRoom){
            // Assertions
            if(room == null) {
                throw new ArgumentNullException("Room can not be null when added to the dungeon");
            }
            else if (connectingRoom == null){
                throw new ArgumentNullException("Connecting room can not be null");
            }
            else if(!this.RoomExists(connectingRoom)){
                throw new ArgumentException("Connecting room does not exist in the cluster");
            }
            else if (this.RoomExists(room)){
                throw new ArgumentException("Room already exists in the cluster");
            }
            else if (!Room.IsRoomAdjacent(room, connectingRoom)){
                throw new ArgumentException("Room must be adjacent to eachother");
            }
            // If the room doesn't already exist in the dungeon, add it.
            if (!dungeon.RoomExists(room)){
                dungeon.AddRoom(room);
            }
            // Records the room cluster into the newley added room.
            room.cluster = clusterNum;
            // Adds the room to the dictionary of rooms inside of the cluster.
            rooms[room.roomCoordinate] = room;
            // Connects the two rooms by a door.
            Door newDoor = new Door(room, connectingRoom, Door.DoorState.open);
            // Adds the door into the collection of roomConnectors in the dungeon.
            dungeon.roomConnectors.Add(newDoor);
            // Updates AdjacentRooms of the cluster and any surrounding cluster
            UpdateAdjacentRooms(room);
        }

        public void UpdateAdjacentRooms(Room room){
            List<Room> adjacentRooms = Dungeon.GetAdjacentRooms(room, dungeon.rooms);
            foreach (Room adjacentRoom in adjacentRooms)
            {
                Cluster adjacentRoomCluster = dungeon.InWhatCluster(adjacentRoom);
                if (adjacentRoomCluster != null && !adjacentRoomCluster.Equals(this)){
                    this.AddAdjacentRoom(room, adjacentRoom);
                    adjacentRoomCluster.AddAdjacentRoom(adjacentRoom,room);
                }
            }
        }
        
        private void AddAdjacentRoom(Room roomThisCluster, Room roomOtherCluster){
            // Assertions
            if(!this.RoomExists(roomThisCluster)){
                throw new ArgumentException("first room must be in the current cluster");
            }
            Cluster otherRoomCluster = dungeon.InWhatCluster(roomOtherCluster);
            if(this.RoomExists(roomOtherCluster) || otherRoomCluster == null){
                throw new ArgumentException("second room must be in an existing cluster other than this one");
            }
            else if(!Room.IsRoomAdjacent(roomThisCluster,roomOtherCluster)){
                throw new ArgumentException("rooms are not adjacent");
            }
            if(roomThisCluster.Equals(dungeon.endRoom) || roomOtherCluster.Equals(dungeon.endRoom)){
                return;
            }
            else if(!this.adjacentClusters.Exists(aCluster => aCluster.Item1.Equals(otherRoomCluster))){
                this.adjacentClusters.Add(
                    (otherRoomCluster,
                    new List<(Room, Room)>(){(roomThisCluster,roomOtherCluster)}));
            }
            else{
                (Cluster,List<(Room,Room)>) roomList = 
                    this.adjacentClusters.Find(aCluster => aCluster.Item1.Equals(otherRoomCluster));
                roomList.Item2.Add((roomThisCluster, roomOtherCluster));
            }
        }

        public List<(Room,Room)> GetAdjacentRoomsOfCluster(Cluster cluster){
            if(!this.adjacentClusters.Exists(aCluster => aCluster.Item1.Equals(cluster))){
                return null;
            }
            (Cluster,List<(Room,Room)>) clusterAndRooms = 
                this.adjacentClusters.Find(aCluster => aCluster.Item1.Equals(cluster));
            return clusterAndRooms.Item2;
        }

        public bool RoomExists(Room room){
            return rooms.ContainsKey(room.roomCoordinate);
        }

        public bool Equals(Cluster cluster){
            if(cluster == null) return false;
            return cluster.clusterNum == this.clusterNum;
        }

        public Room GetRandomRoom(){
            Room[] rooms = Enumerable.ToArray(this.rooms.Values);
            int index = dungeon.rand.Next(rooms.Length);
            return rooms[index];
        }

        // Gets a random room that is adjacent to the cluster.
        public Room GetRandomAdjacentRoomInCluster(Room room)
        {
            List<Room> adjacentRooms = Dungeon.GetAdjacentRooms(room, this.rooms);
            if (adjacentRooms.Count == 0) return null;
            int index = dungeon.rand.Next(adjacentRooms.Count);
            return adjacentRooms[index];
        }

        // Generates an adjacent room to the cluster.
        // The generatedroom is not added to any lists.
        public Room GenerateAdjacentRoomFromCluster(){
            bool invalidRoom;
            Room result;
            do{
                Room borderRoom = GetRandomRoom();
                result = dungeon.GenerateAdjacentRoom(borderRoom);
                if (result == null) invalidRoom = true;
                else invalidRoom = false;
            } while (invalidRoom);
            return result;
        }


        public static Cluster GenerateEndRoomCluster(Dungeon dungeon){
            // Generates the end room (where the chalice is located)
            dungeon.endRoom = dungeon.GenerateRandomRoom();
            dungeon.endRoom.endRoom = true;
            Cluster result = new Cluster(dungeon, dungeon.endRoom);
            dungeon.finalBossRoom = dungeon.GenerateAdjacentRoom(dungeon.endRoom);
            result.AddRoom(dungeon.finalBossRoom,dungeon.endRoom);
            return result;
        }


        // A cluster is defined as a group of rooms that are all connected by doors. 
        // Meaning the player can access every
        // single room in a cluster by going through doors.
        public static Cluster GenerateRandomCluster(
            Dungeon dungeon, Room startingRoom, int numOfRooms, 
            List<RoomCoordinate> excluding = null)
        {
            if (excluding == null){
                excluding = new List<RoomCoordinate>();
            }
            Cluster result = new Cluster(dungeon, startingRoom);
            // Temporary list of possible roomsCoordinates where a new room can be created.
            // The function continuously adds to this list every time a new room is created.
            List<RoomCoordinate> possibleRooms = new List<RoomCoordinate>();
            RoomCoordinate newestRoom = startingRoom.roomCoordinate;
            
            for (int i = numOfRooms; i > 0; i--)
            {
                // Adds new possible surrounding coordinates to the pool of possible coordinates
                // Possible coordinates must be adjecent to a currently made room. 
                RoomCoordinate[] surroundingRCs = RoomCoordinate.SurroundingsRoomCoordinates(newestRoom);
                foreach (RoomCoordinate surroundingRC in surroundingRCs)
                {
                    if (!dungeon.RoomExists(surroundingRC) &&
                    !possibleRooms.Exists(rC => rC.Equals(surroundingRC)) &&
                    dungeon.RCInDungeonBorders(surroundingRC) &&
                    !excluding.Exists(aRC => aRC.Equals(surroundingRC)))
                    {
                        possibleRooms.Add(surroundingRC);
                    }
                }
                // If, for any reason, a new room can not be created, 
                // the function returns the cluster anyway.
                if (possibleRooms.Count <= 0){ Debug.Log("Return Incomplete"); return result;}
                // Picks one coordinate from the pool of possible ones
                int index = dungeon.rand.Next(possibleRooms.Count);
                // Creates new room from that coordinates
                Room newRoom = new Room(possibleRooms[index]);
                // Removes the used coordinate from the pool
                possibleRooms.Remove(possibleRooms[index]);
                // Finds a room that was created in the cluster that is adjecent to the newly created room.
                Room adjacentRoom = result.GetRandomAdjacentRoomInCluster(newRoom);
                // Adds the room to the cluster
                result.AddRoom(newRoom, adjacentRoom);
                newestRoom = newRoom.roomCoordinate;
            }
            return result;
        }
        // Function used to conntect the base room of a cluster to an already existing room in a previous cluster.
        public static Stair ConnectTwoClustersStair(Cluster c1, Cluster c2){
            if (c1.dungeon != c2.dungeon){
                throw new ArgumentException("There shouldn't be two dungeons");
            }
            Dungeon dungeon = c1.dungeon;
            // Assertions
            if (c1 == null || c2 == null){
                throw new ArgumentNullException("c1 or c2 can not be null");
            }
            else if (c1.Equals(dungeon.endRoomsCluster)||c2.Equals(dungeon.endRoomsCluster)){
                throw new ArgumentException("c1 and c2 can not be the endRoomsCluster");
            }

            bool nonValidRoomGot;
            Room room1;
            Room room2;

            // Two conditions for a nonvalid room:
            // 1. The random room == room.
            // 2. The random room already has a stair connection.
            // 3. The random room is not the start room.
            // 4. The random room is not the end room.
            do {
                room1 = c1.GetRandomRoom();
                room2 = c2.GetRandomRoom();
                nonValidRoomGot = 
                    room1.Equals(room2) || 
                    room1.HasStair() || 
                    room2.HasStair() ||
                    room1.Equals(dungeon.startRoom) ||
                    room2.Equals(dungeon.startRoom);
                nonValidRoomGot = nonValidRoomGot || dungeon.RoomConnectionExists(room1, room2);
            } while(nonValidRoomGot);
            return new Stair(room1, room2, new Vector2(6.5f,2.5f), new Vector2(6.5f,2.5f));
        }

        public static Door ConnectTwoClustersDoor(
            Cluster c1, Cluster c2, 
            Door.DoorState state, 
            Door.LockedDoorColor color = Door.LockedDoorColor.none)
            {
            // Assertions
            if (c1.dungeon != c2.dungeon){
                throw new ArgumentException("There shouldn't be two dungeons");
            }
            Dungeon dungeon = c1.dungeon;
            List<(Room,Room)> possibleRoomConnections = c1.GetAdjacentRoomsOfCluster(c2);
            if (possibleRoomConnections.Count <= 0){
                return null;
            }
            bool invalidDoor;
            (Room,Room) roomsToConnect;
            do {
                int index = c1.dungeon.rand.Next(possibleRoomConnections.Count);
                roomsToConnect = possibleRoomConnections[index];
                invalidDoor = 
                    dungeon.RoomConnectionExists(roomsToConnect.Item1, roomsToConnect.Item2);
            } while (invalidDoor);
            return new Door(roomsToConnect.Item1, roomsToConnect.Item2, state, color);
        }
    }


    private List<Cluster> clusters = new List<Cluster>();

    // This cluster represents the last cluster of the dungeon. 
    // The last cluster holds only the end room and the final boss room.
    // The only connection to the end room should be to the final boss room via a door
    // and the final boss room should only have one door that connects to the
    // rest of the dungeon.
    // The creation of this cluster and the function, create endRoomsCluster()
    // makes sure that these conditions are followed.
    private Cluster endRoomsCluster;

    private void Start() 
    {
        rand = new System.Random();
        GenerateDungeon();
        dungeonDisplay.CreateDungeon(this);
    }

    // GenerateDungeon() Generates all the rooms that are in the dungeon 
    // in accordance to the properties of all dungeons.
    // Dungeon Properties:
    // 1. Room(0,-1) can not be generated (this is used for an animation in the beginning of the game).
    // 2. Rooms must fit in the area of the dungeon as defined by the Rect object in this class.
    private void GenerateDungeon()
    {
        int numOfClusters = rand.Next(minClusters, maxClusters + 1);
        CreateEndRoomsCluster();
        numOfClusters --;
        CreateStartingRoomCluster();
        numOfClusters --;
        CreateRestOfRandomClusters(numOfClusters);
        ConnectEndClusterToDungeon();
    }

    private void CreateEndRoomsCluster(){
        endRoomsCluster = Cluster.GenerateEndRoomCluster(this);
        clusters.Add(endRoomsCluster);
    }

    private void CreateStartingRoomCluster(){
        int numOfRoomsForNextCluster = rand.Next(minRoomsPerCluster, maxRoomsPerCluster+1);
        numOfRoomsForNextCluster --;
        startRoom = new Room(0,0, startRoom: true);
        List<RoomCoordinate> roomToExclude = new List<RoomCoordinate>()
            {new RoomCoordinate(startRoom.roomCoordinate.GetVector2()+new Vector2(0,-1))};
        Cluster startCluster = Cluster.GenerateRandomCluster(this, startRoom,numOfRoomsForNextCluster, roomToExclude);
        clusters.Add(startCluster);
    }
    private void CreateRestOfRandomClusters(int numOfClusters){
        for (int numOfClustersLeft = numOfClusters; numOfClustersLeft >= 1; numOfClustersLeft--)
        {
            int numOfRoomsForNextCluster = rand.Next(minRoomsPerCluster, maxRoomsPerCluster+1);
            int doorOrStair = rand.Next(100);
            if (doorOrStair <= (int) (stairDoorRatio * 100)){
                Room newRoom = GenerateRandomRoom();
                AddRoom(newRoom);
                numOfRoomsForNextCluster--;
                Cluster newCluster = Cluster.GenerateRandomCluster(this, newRoom, numOfRoomsForNextCluster);
                Cluster connectingCluster = GetRandomClustereExcluding(newCluster);
                clusters.Add(newCluster);
                Stair newStair = Cluster.ConnectTwoClustersStair(newCluster, connectingCluster);
                roomConnectors.Add(newStair);
            }
            else {
                Debug.Log("Called");
                Cluster connectingCluster = GetRandomCluster();
                Room newRoom = connectingCluster.GenerateAdjacentRoomFromCluster();
                AddRoom(newRoom);
                Cluster newCluster = Cluster.GenerateRandomCluster(this, newRoom, numOfRoomsForNextCluster);
                clusters.Add(newCluster);
                Door newDoor = Cluster.ConnectTwoClustersDoor(connectingCluster, newCluster,Door.DoorState.closed);
                roomConnectors.Add(newDoor);
            }
        }
    }
    private void ConnectEndClusterToDungeon(){
        List<(Cluster,List<(Room,Room)>)> adjacentClustersToEndRoom = 
            endRoomsCluster.adjacentClusters;
        bool mustBeStairs = adjacentClustersToEndRoom.Count == 0;
        int doorOrStair = rand.Next(100);
        if (mustBeStairs || doorOrStair <= (int) (stairDoorRatio * 100)){
            Room adjacentRoomToFinalDungeon = GenerateAdjacentRoom(finalBossRoom);
            Cluster clusterThatConnectsToEndRooms = 
                new Cluster(this, adjacentRoomToFinalDungeon);
            clusters.Add(clusterThatConnectsToEndRooms);
            Door newDoor = Cluster.ConnectTwoClustersDoor(
                endRoomsCluster, 
                clusterThatConnectsToEndRooms, 
                Door.DoorState.locked, 
                Door.LockedDoorColor.yellow);
            roomConnectors.Add(newDoor);
            Cluster clusterThatExistsInDungeon = 
                GetRandomClustereExcluding(clusterThatConnectsToEndRooms);
            Stair newStair = 
                Cluster.ConnectTwoClustersStair(
                    clusterThatExistsInDungeon,
                    clusterThatConnectsToEndRooms);
            roomConnectors.Add(newStair);
        }
        else{
            int index = rand.Next(adjacentClustersToEndRoom.Count);
            Cluster clusterToConnectTo = adjacentClustersToEndRoom[index].Item1;
            Door newDoor = 
                Cluster.ConnectTwoClustersDoor(
                    endRoomsCluster, 
                    clusterToConnectTo, 
                    Door.DoorState.locked, 
                    Door.LockedDoorColor.yellow);
            roomConnectors.Add(newDoor);
        }  
    }

    // Generates a random room as long as it is inside of the borders of the 
    // dungeon and does not yet exist in the dungeon.
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

    private static List<Room> GetAdjacentRooms (
        Room room, 
        Dictionary<RoomCoordinate,Room> roomsToChooseFrom)
    {
        RoomCoordinate[] newSurroundingRCs =
            RoomCoordinate.SurroundingsRoomCoordinates(room.roomCoordinate);
        List<Room> adjacentRooms = new List<Room>();
        foreach (RoomCoordinate RC in newSurroundingRCs)
        {
            if (roomsToChooseFrom.ContainsKey(RC))
            {
                adjacentRooms.Add(roomsToChooseFrom[RC]);
            }
        }
        return adjacentRooms;
    }



    // Checks wether or not the room coordinate is inside of the dungeon's boarders.
    private bool RCInDungeonBorders(RoomCoordinate roomCoordinate)
    {
        int x = roomCoordinate.x;
        int y = roomCoordinate.y;
        bool inX = (x >= (-squareArea / 2) && x <= (squareArea / 2));
        bool inY = (y >= 0 && y <= squareArea);
        return inX && inY;
    }
    // Generates an adjacent room to the room inputed.
    private Room GenerateAdjacentRoom(Room room)
    {
        RoomCoordinate[] surroundingRCs = RoomCoordinate.SurroundingsRoomCoordinates(room.roomCoordinate);
        List<RoomCoordinate> possibleRooms = new List<RoomCoordinate>();
        foreach (RoomCoordinate surroundingRC in surroundingRCs)
        {
            if (!RoomExists(surroundingRC) &&
            RCInDungeonBorders(surroundingRC))
            {
                possibleRooms.Add(surroundingRC);
            }
        }
        if (possibleRooms.Count == 0){
            return null;
        }
        int index = rand.Next(possibleRooms.Count);
        return new Room (possibleRooms[index]);
    }

    protected void AddRoom(Room room)
    {
        if (room == null) throw new ArgumentNullException("added room can not be null");
        rooms[room.roomCoordinate] = room;
    }

    private Cluster GetCluster(int clusterNum)
    {
        return clusters.Find(aCluster => aCluster.clusterNum == clusterNum);
    }

    private Cluster InWhatCluster(Room room)
    {
        return clusters.Find(aCluster => aCluster.RoomExists(room));
    }
    
    private Cluster GetRandomCluster(){
        if (clusters.Count == 0){
            throw new InvalidOperationException("No clusters exists");
        }
        bool invalidCluster;
        Cluster result;
        do{
            int index = rand.Next(clusters.Count);
            result = clusters[index];
            invalidCluster = result.Equals(endRoomsCluster);
            if (invalidCluster && clusters.Count == 1){
                throw new InvalidOperationException("Only end cluster exists");
            }
        } while (invalidCluster);
        return result;
    }

    private Cluster GetRandomClustereExcluding(Cluster cluster){
        bool invalidCluster;
        Cluster result;
        do{
            result = GetRandomCluster();
            invalidCluster = result.Equals(cluster);
            if (invalidCluster && clusters.Count == 2){
                throw new InvalidOperationException("Only two cluster exists");
            }
        } while (invalidCluster);
        return result;
    }

    // GetRoom is the Room that has the RoomCoordinate coordinate.
    // coordinate must be a coordinate that exists in the dungeon.
    public Room GetRoom(RoomCoordinate coordinate)
    {
        return rooms[coordinate];
    }

    public Room GetRoom(int x, int y)
    {
        return GetRoom(new RoomCoordinate(x, y));
    }

    public Room[] GetAllRooms()
    {
        return Enumerable.ToArray(rooms.Values);
    }

    public bool RoomExists(RoomCoordinate coordinate)
    {
        return rooms.ContainsKey(coordinate);
    }

    public bool RoomExists(Room room)
    {
        return rooms.ContainsKey(room.roomCoordinate);
    }

    public bool RoomConnectionExists(RoomConnector roomConnector){
        return roomConnectors.Exists(aRoomConnector => aRoomConnector.Equals(roomConnector));
    }

    public bool RoomConnectionExists(Room room1, Room room2){
        return roomConnectors.Exists(aRoomConnector => aRoomConnector.Equals(room1, room2));
    }

}