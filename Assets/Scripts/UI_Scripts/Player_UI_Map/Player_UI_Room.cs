using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_UI_Room : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Image roomSprite;
    [SerializeField] Player_UI_Door[] doors;
    
    public Room room {get; private set;}

    public void SetRoom(Room room){
        this.room = room;   
    }

    public void DisplayRoom(){
        if (room == null){
            return;
        }
        
        this.gameObject.SetActive(true);
        foreach (Door door in room.doors){
            DisplayDoor(door);
        }
    }

    private void DisplayDoor(Door door){
        Room nextRoom = door.GetNextRoom(room);
        Vector2 vectorBetweenRooms =
            nextRoom.roomCoordinate.GetVector2() -
            room.roomCoordinate.GetVector2();
        Cardinal4.Direction direction =
            Cardinal4.Vector2ToDirection(vectorBetweenRooms);
        
        doors[(int) direction].DisplayDoor();
    }
}
