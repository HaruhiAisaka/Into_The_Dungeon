using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonDisplayDebugger : DungeonDisplay
{
    private bool draw = false;

    private void Update() {
        if(draw){
            foreach (RoomConnector connector in dungeon.roomConnectors){
            if (connector is Stair stair){
                Vector3[] positions = 
                    {stair.room1.roomCoordinate.GetRoomWorldPosition()+stair.stairPosition1, 
                    stair.room2.roomCoordinate.GetRoomWorldPosition()+stair.stairPosition2};
                Debug.DrawLine(positions[0], positions[1], Color.red, 0);
                }
            }
        }

    }
    public override void CreateDungeon(Dungeon dungeon){
        this.dungeon = dungeon;
        // Create a dummy room and door.
        Room startRoom = dungeon.startRoom;
        Vector2 coordinateForDummyRoom = 
            startRoom.roomCoordinate.GetVector2() + new Vector2(0,-1);
        Room dummyRoom = new Room(new RoomCoordinate(coordinateForDummyRoom));
        Door dummyDoor = new Door(dummyRoom, startRoom, Door.DoorState.closed);
        currentRoomGenerator = InstantiateRoom(dungeon.startRoom);
        player.GetComponent<CurrentRoom>().SetCurrentRoom(dungeon.startRoom);
        player.transform.position = dungeon.startRoom.roomCoordinate.GetRoomWorldPosition();
        mainCamera.SetCameraToNewRoom(dungeon.startRoom);
        currentRoomGenerator.EnableDoorAnimations(true);
        Destroy(curtains.gameObject);
    }

    [ContextMenu("InstantiateAllRooms")]
    private void InstantiateAllRooms(){
        foreach (Room room in dungeon.GetAllRooms())
        {
            RoomGenerator newRoomGenerator = InstantiateRoom(room);
            newRoomGenerator.EnableDebugOverlays(true);
        }
        draw = true;
    }

    [ContextMenu("DeleteAllRooms")]
    private void DeleteAllRooms(){
        RoomGenerator[] rooms = FindObjectsOfType<RoomGenerator>();
        foreach(RoomGenerator room in rooms){
            Destroy(room.gameObject);
        }
        Room currentRoom = FindObjectOfType<CurrentRoom>().currentRoom;
        InstantiateRoom(currentRoom);
        draw = false;
    }

}
