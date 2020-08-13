using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonDisplay : MonoBehaviour
{
    private Dungeon dungeon;
    [SerializeField] private RoomGenerator roomGenerator;
    [SerializeField] private Curtains curtains;

    private RoomGenerator currentRoomGenerator;


    public void CreateDungeon(Dungeon dungeon){
        this.dungeon = dungeon;
        currentRoomGenerator = InstantiateRoom(dungeon.startRoom);
        currentRoomGenerator.EnableDoorAnimations(false);
        StartCoroutine(EnterDungeonAnimation());
    }


    public RoomGenerator InstantiateRoom(Room room){
        RoomGenerator newRoom = Instantiate(roomGenerator);
        newRoom.GenerateRoom(room);
        currentRoomGenerator = newRoom;
        return newRoom;
    }

    [ContextMenu("InstantiateAllRooms")]
    private void InstantiateAllRooms(){
        foreach (KeyValuePair<RoomCoordinate,Room> keyValue in dungeon.rooms)
        {
            Room room = keyValue.Value;
            InstantiateRoom(room);
        }
    }

    [ContextMenu("DeleteAllRooms")]
    private void DeleteAllRooms(){
        RoomGenerator[] rooms = FindObjectsOfType<RoomGenerator>();
        foreach(RoomGenerator room in rooms){
            Destroy(room.gameObject);
        }
        Room currentRoom = FindObjectOfType<CurrentRoom>().currentRoom;
        InstantiateRoom(currentRoom);
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
        Vector2 movePlayerHere = dungeon.startRoom.roomCoordinate.GetRoomWorldPosition() + new Vector2(0,-2.5f);
        Coroutine b = StartCoroutine(player.MovePlayerToPoint(movePlayerHere, player.speed));
        yield return b;
        Door dummyDoor = (Door) dungeon.startRoom.roomConnectors.Find(door => door.HasRoom(new RoomCoordinate(0,-1)));
        dummyDoor.ChangeState(Door.DoorState.closed);
        player.UnfreezePlayer();
        currentRoomGenerator.EnableDoorAnimations(true);
        currentRoomGenerator.EnableStairAnimations(true);
    }
}
