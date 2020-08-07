using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dungeon : MonoBehaviour
{
    private static RoomCoordinateEqual roomCoorEqual = 
        new RoomCoordinateEqual();
    [SerializeField] private RoomGenerator roomGenerator;

    [SerializeField] private Curtains curtains;

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
    }

    private void Start() {
        currentRoomGenerator = InstantiateRoom(startRoom);
        currentRoomGenerator.EnableDoorAnimations(false);
        StartCoroutine(EnterDungeonAnimation());
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


    private IEnumerator EnterDungeonAnimation(){
        Player player = FindObjectOfType<Player>();
        player.FreezePlayer();
        player.UnfreezeAnimation();
        player.GetComponent<Animator>().SetFloat("deltaY", 1);
        player.GetComponent<Animator>().speed = 1;
        curtains.SetCurtainsClosed();
        Coroutine a = 
            StartCoroutine(curtains.OpenCurtains());
        yield return a;
        Destroy(curtains.gameObject);
        player.transform.position = new Vector2(0, -5);
        Vector2 movePlayerHere = startRoom.roomCoordinate.GetRoomWorldPosition() + new Vector2(0,-2.5f);
        Coroutine b = StartCoroutine(player.MovePlayerToPoint(movePlayerHere, player.speed));
        yield return b;
        Door dummyDoor = (Door) startRoom.roomConnectors.Find(door => door.HasRoom(new RoomCoordinate(0,-1)));
        dummyDoor.ChangeState(Door.DoorState.closed);
        player.UnfreezePlayer();
        currentRoomGenerator.EnableDoorAnimations(true);
        currentRoomGenerator.EnableStairAnimations(true);
    }
}
