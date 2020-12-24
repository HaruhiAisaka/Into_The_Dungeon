using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dungeon : MonoBehaviour
{
    [SerializeField] protected ItemDatabase itemDatabase;

    [SerializeField] protected DungeonDisplay dungeonDisplay;

    [SerializeField] protected PlayerUI playerUI;

    [SerializeField] protected int squareArea = 9;

    [Header("Procedural Generation Parameters")]
    [SerializeField] [Range(1,30)] protected int minRoomsPerCluster = 20;
    [SerializeField] [Range(1,30)] protected int maxRoomsPerCluster = 20;

    [SerializeField] [Range(2,10)] protected int minClusters = 1;
    [SerializeField] [Range(2,10)] protected int maxClusters = 1;

    [SerializeField] [Range(0,1)] protected float stairDoorRatio = .2f;

    [SerializeField] protected Vector2 positionOfItems = new Vector2(0,0);

    private static List<String> enemies = new List<string>(){"skull", "blob", "bat", "shooter"};

    public Dictionary<RoomCoordinate, Room> rooms {get; private set;} = 
        new Dictionary<RoomCoordinate, Room>(new RoomCoordinateEqual());

    public List<RoomConnector> roomConnectors { get; protected set; } =
        new List<RoomConnector>();
 
    public Room startRoom { get; protected set;}
    protected Room finalBossRoom;
    protected Room endRoom;

    // A list of items that the dungeon needs to have in order for the player to be able to advance.
    protected List<Item> neededItems = new List<Item>();


    // A cluster is a series of rooms all connected by open doors.
    // Meaning that the player can enter into all the rooms without the need of keys to open locked doors.
    // This distinction is used to simplify and further abstract the procedural generation algorithm.
    public class Cluster{
        private Dungeon dungeon;
        public Dictionary<RoomCoordinate, Room> rooms {get; private set;} = 
            new Dictionary<RoomCoordinate, Room>(new RoomCoordinateEqual());
        public int clusterNum {get; private set;}
        private static int nextClusterNum = 0;

        // Distance to the starting cluster. 
        // The int represents how many cluster connections between it and the start cluster.
        public int distanceToStartCluster = -1;

        public static int maxDistanceToStartCluster = 0;

        // List of items the player needs to access the cluster.
        public List<Item> neededItemsToAccessCluster {get; private set;} = new List<Item>();
        public List<Item> itemsInCluster {get; private set;} = new List<Item>();

        // Room adjacentRoom is a room outside of the current cluster that is adjacent to the cluster. 
        // This information is used when two clusters are ment to be connected by door.
        public List<(Cluster,List<(Room,Room)>)> adjacentClusters {get; private set;} =
            new List<(Cluster, List<(Room, Room)>)>();

        public List<(Cluster,List<RoomConnector>)> connectedClusters {get; private set;} =
            new List<(Cluster, List<RoomConnector>)>();
        public Cluster(Dungeon dungeon, Room startingRoom){
            this.dungeon = dungeon;
            this.clusterNum = nextClusterNum;
            nextClusterNum ++;
            dungeon.clusters.Add(this);
            if(startingRoom == null){
                throw new System.ArgumentException("startingRoom can not be null");
            }
            // If the room doesn't already exist in the dungeon, add it.
            if (!dungeon.RoomExists(startingRoom)){
                dungeon.AddRoom(startingRoom);
            }
            if (startingRoom.Equals(dungeon.startRoom)){
                distanceToStartCluster = 0;
            }
            startingRoom.cluster = this.clusterNum;
            rooms[startingRoom.roomCoordinate] = startingRoom;
            UpdateAdjacentRooms(startingRoom);
        }

        public void AddRoom(Room room, Room connectingRoom){
            // Assertions
            if(room == null) {
                throw new ArgumentNullException("Room can not be null when added to the cluster");
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

        private void AddConnectedCluster(Cluster clusterToConnectTo, RoomConnector roomConnector){
            if(!this.connectedClusters.Exists(aCluster => aCluster.Item1.Equals(clusterToConnectTo))){
                this.connectedClusters.Add(
                    (clusterToConnectTo,
                    new List<RoomConnector>(){roomConnector}));
            }
            else{
                (Cluster,List<RoomConnector>) roomList = 
                    this.connectedClusters.Find(aCluster => aCluster.Item1.Equals(clusterToConnectTo));
                roomList.Item2.Add(roomConnector);
            }
            if(clusterToConnectTo.distanceToStartCluster != -1 && (this.distanceToStartCluster == -1 ||
            this.distanceToStartCluster > clusterToConnectTo.distanceToStartCluster))
            {
                this.distanceToStartCluster = clusterToConnectTo.distanceToStartCluster + 1;
                this.neededItemsToAccessCluster = 
                    this.neededItemsToAccessCluster.Union(clusterToConnectTo.neededItemsToAccessCluster).ToList();
                if(this.distanceToStartCluster > maxDistanceToStartCluster){
                    maxDistanceToStartCluster = this.distanceToStartCluster;
                }
            }
        }

        public bool RoomExists(Room room){
            return rooms.ContainsKey(room.roomCoordinate);
        }

        public bool Equals(Cluster cluster){
            if(cluster == null) return false;
            return cluster.clusterNum == this.clusterNum;
        }

        public Room GetRandomRoom(){
            return RNG.RandomElementFromDictionary(rooms);
        }

        // Gets a random room that is adjacent to the cluster.
        public Room GetRandomAdjacentRoomInCluster(Room room)
        {
            List<Room> adjacentRooms = Dungeon.GetAdjacentRooms(room, this.rooms);
            return RNG.RandomElementFromList(adjacentRooms);
        }

        public bool IsClusterEnclosed(){
            List<Room> validRooms = 
                rooms.Values.Where(
                    aRoom => dungeon.HasSpaceForAdjacentRoom(aRoom)).ToList();
            if (validRooms.Contains(dungeon.endRoom)){
                validRooms.Remove(dungeon.endRoom);
            }
            return validRooms.Count == 0;
        }

        // Generates an adjacent room to the cluster.
        // The generatedroom is not added to any lists.
        public Room GenerateAdjacentRoomFromCluster(){
            Room[] validRooms = 
                rooms.Values.Where(
                    aRoom => dungeon.HasSpaceForAdjacentRoom(aRoom) && 
                    !aRoom.Equals(dungeon.endRoom)).ToArray();
            if(validRooms.Length == 0){
                return null;
            }
            Room boarderRoom = RNG.RandomElementFromList(validRooms);
            return dungeon.GenerateAdjacentRoom(boarderRoom);
        }

        public static Cluster GenerateEndRoomCluster(Dungeon dungeon){
            // Generates the end room (where the chalice is located)
            dungeon.endRoom = dungeon.GenerateRandomRoom();
            dungeon.endRoom.endRoom = true;
            Cluster result = new Cluster(dungeon, dungeon.endRoom);
            dungeon.finalBossRoom = dungeon.GenerateAdjacentRoom(dungeon.endRoom);
            result.AddRoom(dungeon.finalBossRoom,dungeon.endRoom);
            result.AddNeededItem(dungeon.itemDatabase.GetKey(Door.LockedDoorColor.gold));
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
                RoomCoordinate newRoomCoordinate = RNG.RandomElementFromList(possibleRooms);
                // Creates new room from that coordinates
                Room newRoom = new Room(newRoomCoordinate);
                // Removes the used coordinate from the pool
                possibleRooms.Remove(newRoomCoordinate);
                // Finds a room that was created in the cluster that is adjecent to the newly created room.
                Room adjacentRoom = result.GetRandomAdjacentRoomInCluster(newRoom);
                // Adds the room to the cluster
                result.AddRoom(newRoom, adjacentRoom);
                newestRoom = newRoom.roomCoordinate;
            }
            return result;
        }
        // Function used to conntect the base room of a cluster to an already existing room in a previous cluster.
        public static Stair StairThatConnectsTwoClusters(Cluster c1, Cluster c2){
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
            return new Stair(room1, room2, new Vector2(6.5f,3f), new Vector2(6.5f,3f));
        }

        public static Door DoorThatConnectsTwoClusters(
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
            List<(Room,Room)> invalidRoomConnections = 
                dungeon.roomConnectors.Select(aRoomConnector => aRoomConnector.GetRoomTuplet()).ToList();
            (Room, Room) roomsToConnect = RNG.RandomElementFromListExcluding(possibleRoomConnections, invalidRoomConnections);
            return new Door(roomsToConnect.Item1, roomsToConnect.Item2, state, color);
        }

        public static void ConfirmClusterConnection(Cluster c1, Cluster c2, RoomConnector roomConnector){
            c1.AddConnectedCluster(c2, roomConnector);
            c2.AddConnectedCluster(c1, roomConnector);
            if(roomConnector is Door door && door.color != Door.LockedDoorColor.none){
                Key key = c1.dungeon.itemDatabase.GetKey(door.color);
                if(c1.distanceToStartCluster > c2.distanceToStartCluster){
                    c1.AddNeededItem(key);
                }
                else if(c2.distanceToStartCluster > c1.distanceToStartCluster){
                    c2.AddNeededItem(key);
                }
            }
        }

        public void AddNeededItem(Item item){
            neededItemsToAccessCluster.Add(item);
            if(!dungeon.neededItems.Contains(item)){
                dungeon.neededItems.Add(item);
            }
        }

        public void AddItemToRandomRoom(Item item){
            itemsInCluster.Add(item);
            Room[] possibleRooms = 
                rooms.Values.Where(aRoom => aRoom.items.Count <= 0).ToArray();
            Room[] forbidenRooms = {dungeon.startRoom, dungeon.endRoom, dungeon.finalBossRoom};
            if(possibleRooms.Length == 0){
                possibleRooms = rooms.Values.ToArray();
            }
            Room chosenRoom = RNG.RandomElementFromListExcluding(possibleRooms, forbidenRooms);
            chosenRoom.AddItem(item, new Vector2(0,0));
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

    private Cluster clusterThatConnectsToEndRooms;

    private void Start() 
    {
        Debug.Log("RANDOM NUMBER GENERATOR SEED: " + RNG.seed);
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
        int numOfClusters = RNG.Next(minClusters, maxClusters + 1);
        CreateEndRoomsCluster();
        numOfClusters --;
        CreateStartingRoomCluster();
        numOfClusters --;
        CreateRestOfRandomClusters(numOfClusters);
        ConnectEndClusterToDungeon();
        AddItems();
        playerUI.SetMap();
    }

    private void CreateEndRoomsCluster(){
        endRoomsCluster = Cluster.GenerateEndRoomCluster(this);
    }

    private void CreateStartingRoomCluster(){
        int numOfRoomsForNextCluster = RNG.Next(minRoomsPerCluster, maxRoomsPerCluster+1);
        numOfRoomsForNextCluster --;
        startRoom = new Room(0,0, startRoom: true);
        List<RoomCoordinate> roomToExclude = new List<RoomCoordinate>()
            {new RoomCoordinate(startRoom.roomCoordinate.GetVector2()+new Vector2(0,-1))};
        Cluster startCluster = Cluster.GenerateRandomCluster(this, startRoom,numOfRoomsForNextCluster, roomToExclude);
    }
    private void CreateRestOfRandomClusters(int numOfClusters){
        for (int numOfClustersLeft = numOfClusters; numOfClustersLeft >= 1; numOfClustersLeft--)
        {
            int numOfRoomsForNextCluster = RNG.Next(minRoomsPerCluster, maxRoomsPerCluster+1);
            int doorOrStair = RNG.Next(100);
            if (doorOrStair <= (int) (stairDoorRatio * 100)){
                Room newRoom = GenerateRandomRoom();
                AddRoom(newRoom);
                numOfRoomsForNextCluster--;
                Cluster newCluster = Cluster.GenerateRandomCluster(this, newRoom, numOfRoomsForNextCluster);
                Cluster connectingCluster = GetRandomClusterExcluding(new List<Cluster>() {newCluster});
                Stair newStair = Cluster.StairThatConnectsTwoClusters(newCluster, connectingCluster);
                ConfirmClusterConnection(newCluster, connectingCluster, newStair);
            }
            else {
                Cluster[] possibleConnectingClusters = clusters.Where(
                    aCluster => !aCluster.IsClusterEnclosed() && !aCluster.Equals(endRoomsCluster)).ToArray();
                Cluster connectingCluster = RNG.RandomElementFromList(possibleConnectingClusters);
                Room newRoom = connectingCluster.GenerateAdjacentRoomFromCluster();
                Cluster newCluster = Cluster.GenerateRandomCluster(this, newRoom, numOfRoomsForNextCluster);
                Door.LockedDoorColor doorColor = 
                    RNG.RandomEnumValueExcluding<Door.LockedDoorColor>(new Door.LockedDoorColor[]
                    {Door.LockedDoorColor.gold, Door.LockedDoorColor.none});
                Door newDoor = Cluster.DoorThatConnectsTwoClusters(connectingCluster, newCluster,Door.DoorState.locked, doorColor);
                ConfirmClusterConnection(newCluster, connectingCluster, newDoor);
            }
        }
    }
    private void ConnectEndClusterToDungeon(){
        List<(Cluster,List<(Room,Room)>)> adjacentClustersToEndRoom = 
            endRoomsCluster.adjacentClusters;
        bool mustBeStairs = adjacentClustersToEndRoom.Count == 0;
        bool mustBeDoor = !HasSpaceForAdjacentRoom(finalBossRoom);
        int doorOrStair = RNG.Next(100);
        if (mustBeStairs || (!mustBeDoor && doorOrStair <= (int) (doorOrStair * 100)))
        {
            Room adjacentRoomToFinalDungeon = GenerateAdjacentRoom(finalBossRoom);
            clusterThatConnectsToEndRooms = 
                new Cluster(this, adjacentRoomToFinalDungeon);
            Door newDoor = Cluster.DoorThatConnectsTwoClusters(
                endRoomsCluster, 
                clusterThatConnectsToEndRooms, 
                Door.DoorState.locked, 
                Door.LockedDoorColor.gold);
            ConfirmClusterConnection(endRoomsCluster, clusterThatConnectsToEndRooms, newDoor);

            Cluster clusterThatExistsInDungeon = 
                GetRandomClusterExcluding(new List<Cluster>() {clusterThatConnectsToEndRooms,endRoomsCluster});
            Stair newStair = 
                Cluster.StairThatConnectsTwoClusters(
                    clusterThatExistsInDungeon,
                    clusterThatConnectsToEndRooms);
            ConfirmClusterConnection(clusterThatExistsInDungeon, clusterThatConnectsToEndRooms, newStair);
            
        }
        else{
            Cluster clusterToConnectTo = RNG.RandomElementFromList(adjacentClustersToEndRoom).Item1;
            Door newDoor = 
                Cluster.DoorThatConnectsTwoClusters(
                    endRoomsCluster, 
                    clusterToConnectTo, 
                    Door.DoorState.locked, 
                    Door.LockedDoorColor.gold);
            ConfirmClusterConnection(endRoomsCluster, clusterToConnectTo, newDoor);
        }  
    }

    private void ConfirmClusterConnection(Cluster c1, Cluster c2, RoomConnector roomConnector){
        if(!(c1.RoomExists(roomConnector.room1) && c2.RoomExists(roomConnector.room2)) &&
        !(c1.RoomExists(roomConnector.room2) && c2.RoomExists(roomConnector.room1)))
        {
            throw new System.ArgumentException("Room Connector does not connect the two clusters");
        }
        roomConnectors.Add(roomConnector);
        Cluster.ConfirmClusterConnection(c1, c2, roomConnector);
    }

    private void AddItems(){
        AddNeededItems();
    }

    private void AddNeededItems(){
        foreach (Item item in neededItems)
        {
            List<Cluster> possibleClusters = 
                clusters.Where(aCluster => !aCluster.neededItemsToAccessCluster.Contains(item)).ToList();
            if(clusterThatConnectsToEndRooms != null && 
            possibleClusters.Contains(clusterThatConnectsToEndRooms))
            {
                possibleClusters.Remove(clusterThatConnectsToEndRooms);
            }
            int maxClusterDistance = 
                possibleClusters.Max(aCluster => aCluster.distanceToStartCluster);
            Cluster chosenCluster = 
                possibleClusters.Find(aCluster => aCluster.distanceToStartCluster == maxClusterDistance);
            chosenCluster.AddItemToRandomRoom(item);
            Cluster[] clusterThatNeedItem = 
                clusters.Where(aCluster => aCluster.neededItemsToAccessCluster.Contains(item)).ToArray();
            foreach (Cluster cluster in clusterThatNeedItem){
                foreach (Item neededItem in chosenCluster.neededItemsToAccessCluster)
                {
                    cluster.AddNeededItem(neededItem);
                }
            }
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
            int x = RNG.Next(-squareArea / 2, squareArea / 2 + 1);
            int y = RNG.Next(0, squareArea + 1);
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

    private bool HasSpaceForAdjacentRoom(Room room){
        RoomCoordinate[] surroundingRCs = RoomCoordinate.SurroundingsRoomCoordinates(room.roomCoordinate);
        List<RoomCoordinate> possibleRooms = 
            surroundingRCs.Where(
                aRoomCoordinate => !RoomExists(aRoomCoordinate) && 
                RCInDungeonBorders(aRoomCoordinate)).
            ToList();
        return possibleRooms.Count != 0;
    }

    // Generates an adjacent room to the room inputed.
    private Room GenerateAdjacentRoom(Room room)
    {
        RoomCoordinate[] surroundingRCs = RoomCoordinate.SurroundingsRoomCoordinates(room.roomCoordinate);
        List<RoomCoordinate> possibleRooms = 
            surroundingRCs.Where(
                aRoomCoordinate => !RoomExists(aRoomCoordinate) && 
                RCInDungeonBorders(aRoomCoordinate)).
            ToList();
        if (possibleRooms.Count == 0){
            return null;
        }
        return new Room (RNG.RandomElementFromList(possibleRooms));
    }

    protected void AddRoom(Room room)
    {
        if (room == null) throw new ArgumentNullException("added room can not be null");
        rooms[room.roomCoordinate] = room;
    }


    // Generate a list of enemies to put in the room.
    // TODO : number of enemies depends on the cluster
    private static List<string> GenerateEnemies(){
        List<string> roomEnemies = new List<string>();
        int numEnemies = RNG.Next(1,4);
        for (int i = 0; i < numEnemies; i++){
            roomEnemies.Add(RNG.RandomElementFromList(enemies));
        }
        return roomEnemies;
    }
    private Cluster GetCluster(int clusterNum)
    {
        return clusters.Find(aCluster => aCluster.clusterNum == clusterNum);
    }
    private Cluster InWhatCluster(Room room)
    {
        return clusters.Find(aCluster => aCluster.RoomExists(room));
    }

    private Cluster GetRandomClusterExcluding(List<Cluster> excluding){
        if (!excluding.Exists(aCluster => aCluster.Equals(endRoomsCluster))){
            excluding.Add(endRoomsCluster);
        }
        return RNG.RandomElementFromListExcluding(clusters, excluding);
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