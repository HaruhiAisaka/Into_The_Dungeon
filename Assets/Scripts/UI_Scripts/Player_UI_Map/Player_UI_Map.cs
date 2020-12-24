using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_UI_Map : MonoBehaviour
{
    [SerializeField] Dungeon dungeon;

    [SerializeField] int squareArea = 9;

    [SerializeField] Player_UI_Room player_UI_room_prefab;
    private Dictionary<RoomCoordinate,Player_UI_Room> _rooms = 
        new Dictionary<RoomCoordinate, Player_UI_Room>();

    public void SetMap(){
        Rect mapRect = this.GetComponent<RectTransform>().rect;
        Rect roomRect = player_UI_room_prefab.GetComponent<RectTransform>().rect;
        int widthInRooms = (int) (mapRect.width / roomRect.width);
        int heightInRooms = (int) (mapRect.height / roomRect.height);
        foreach (Room room in dungeon.GetAllRooms())
        {
            Debug.Log(room);
            Player_UI_Room newRoom = Instantiate(player_UI_room_prefab, this.gameObject.transform);
            _rooms[room.roomCoordinate] = newRoom;
            int x = (int) (room.roomCoordinate.x * roomRect.width);
            int y = (int) ((room.roomCoordinate.y * roomRect.height) - mapRect.height / 2 + roomRect.height/2);
            newRoom.transform.localPosition = new Vector2(x,y);
        }
    }

    public void RevealRoom(Room room){
        GetUI_Room(room).DisplayRoom();
    }

    public void RevealAllRooms(){
        foreach (Player_UI_Room room in _rooms.Values)
        {  
           room.DisplayRoom(); 
        }
    }
    
    // Given a room, return the appropreate UI element 
    // that represents the location of that room
    private Player_UI_Room GetUI_Room(Room room){
        return _rooms[room.roomCoordinate];
    }
}
