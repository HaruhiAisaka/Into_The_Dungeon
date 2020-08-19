using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonDisplay : MonoBehaviour
{
    protected Dungeon dungeon;
    [SerializeField] protected RoomGenerator roomGenerator;
    [SerializeField] protected Curtains curtains;

    [SerializeField] protected CameraMovement mainCamera;
    [SerializeField] protected Player player;

    protected RoomGenerator currentRoomGenerator;


    public virtual void CreateDungeon(Dungeon dungeon){
        this.dungeon = dungeon;
        currentRoomGenerator = InstantiateRoom(dungeon.startRoom);
        currentRoomGenerator.EnableDoorAnimations(false);
        player.GetComponent<CurrentRoom>().SetCurrentRoom(dungeon.startRoom);
        mainCamera.SetCameraToNewRoom(dungeon.startRoom);
        StartCoroutine(EnterDungeonAnimation());
    }


    public RoomGenerator InstantiateRoom(Room room){
        RoomGenerator newRoom = Instantiate(roomGenerator);
        newRoom.GenerateRoom(room);
        currentRoomGenerator = newRoom;
        return newRoom;
    }

    private IEnumerator EnterDungeonAnimation(){
        // Create a dummy room and door.
        Room startRoom = dungeon.startRoom;
        Vector2 coordinateForDummyRoom = 
            startRoom.roomCoordinate.GetVector2() + new Vector2(0,-1);
        Room dummyRoom = new Room(new RoomCoordinate(coordinateForDummyRoom));
        Door dummyDoor = new Door(dummyRoom, startRoom, Door.DoorState.open);
        // Player stuff & setting up the cutrains
        player.FreezePlayer();
        player.UnfreezeAnimation();
        player.GetComponent<Animator>().SetFloat("deltaY", 1);
        player.GetComponent<Animator>().speed = 1;
        curtains.SetCurtainsClosed();
        // Begin animation
        Coroutine a = 
            StartCoroutine(curtains.OpenCurtains());
        yield return a;
        Destroy(curtains.gameObject);
        player.transform.position = dungeon.startRoom.roomCoordinate.GetRoomWorldPosition() - new Vector2(0,-5);
        Vector2 movePlayerHere = dungeon.startRoom.roomCoordinate.GetRoomWorldPosition() + new Vector2(0,-2.5f);
        Coroutine b = StartCoroutine(player.MovePlayerToPoint(movePlayerHere, player.speed));
        yield return b;
        dummyDoor.ChangeState(Door.DoorState.closed);
        player.UnfreezePlayer();
        currentRoomGenerator.EnableDoorAnimations(true);
        currentRoomGenerator.EnableStairAnimations(true);
    }
}
