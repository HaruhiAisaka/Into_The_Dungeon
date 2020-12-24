using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_UI_Map : MonoBehaviour
{
    [SerializeField] Player_UI_Room[] rooms;
    [SerializeField] Dungeon dungeon;

    [SerializeField] int squareArea = 9;

    public void SetMap(){
        foreach (Room room in dungeon.GetAllRooms())
        {
            GetUI_Room(room).SetRoom(room);
        }
    }

    public void RevealRoom(Room room){
        GetUI_Room(room).DisplayRoom();
    }

    public void RevealAllRooms(){
        foreach (Player_UI_Room room in rooms)
        {  
           room.DisplayRoom(); 
        }
    }
    
    // Given a room, return the appropreate UI element 
    // that represents the location of that room
    private Player_UI_Room GetUI_Room(Room room){
        int index = room.roomCoordinate.y * squareArea + room.roomCoordinate.x;
        return rooms[index];
    }
}
