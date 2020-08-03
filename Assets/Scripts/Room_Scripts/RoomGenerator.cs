using System;
using UnityEngine;

public class RoomGenerator : MonoBehaviour
{
    private RoomCoordinate roomCoordinate;
    private Room room;
    [SerializeField] private DoorDisplay[] doorDisplays;
    public void GenerateRoom(Room room){
        this.room = room;
        roomCoordinate = room.roomCoordinate;
        this.transform.position = roomCoordinate.GetRoomWorldPosition();
        GenerateRoomConnections();
    }

    private void GenerateRoomConnections(){
        foreach (RoomConnector connector in room.roomConnectors){
            if (connector is Door){
                GenerateDoor((Door) connector);
            }
        }
    }
    private void GenerateDoor(Door door){
        Room nextRoom = door.GetNextRoom(room);
        Vector2 vectorBetweenRooms = 
            nextRoom.roomCoordinate.GetVector2() - 
            room.roomCoordinate.GetVector2();
        Cardinal4.Direction direction = 
            Cardinal4.Vector2ToDirection(vectorBetweenRooms);
        DoorDisplay doorDisplay = 
            Array.Find(doorDisplays, 
            element => element.direction == direction);
        doorDisplay.door = door;
    }

        // Activates the hit boxes that trigger door animations.
    public void EnableDoorAnimations(){
        foreach (DoorDisplay doorDisplay in doorDisplays){
            doorDisplay.EnableAnimations();
        }
    }
}
