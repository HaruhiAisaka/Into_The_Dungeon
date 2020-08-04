using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGenerator : MonoBehaviour
{
    private RoomCoordinate roomCoordinate;
    private Room room;
    [SerializeField] private DoorDisplay[] doorDisplays;
    [SerializeField] private StairDisplay stairDisplay;
    private List<StairDisplay> stairDisplays = 
        new List<StairDisplay>();
    private Player player;

    public void GenerateRoom(Room room){
        player = FindObjectOfType<Player>();
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
            else if(connector is Stair){
                GenerateStairs((Stair) connector);
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

    private void GenerateStairs(Stair stair){
        StairDisplay newStair = Instantiate(stairDisplay, this.transform);
        newStair.transform.localPosition = (Vector3) stair.GetStairPosition(room);
        newStair.stair = stair;

        stairDisplays.Add(newStair);
    }

    // Activates or deactivates the hit boxes that trigger door animations.
    public void EnableDoorAnimations(bool enable){
        foreach (DoorDisplay doorDisplay in doorDisplays){
            doorDisplay.EnableAnimations(enable);
        }
    }

    // Activates or deactivates the hit boxes that trigger stair animations.
    public void EnableStairAnimations(bool enable){
        foreach (StairDisplay stairDisplay in stairDisplays){
            stairDisplay.EnableAnimations(enable);
        }
    }
}
