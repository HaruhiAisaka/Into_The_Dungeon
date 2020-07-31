using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/* Current room info is responcible for storing all the 
information reguarding the current room of the player. 
This includes:
    - The roomCoordinate relative to the origin (starting) room.
*/
public class CurrentRoom : MonoBehaviour
{
    
    [SerializeField] private Room currentRoom;

    private Player player;
    private Dungeon dungeon;
    
    // Start is called before the first frame update
    void Awake()
    {
        dungeon = FindObjectOfType<Dungeon>();
        player = FindObjectOfType<Player>();
        currentRoom = dungeon.GetStartRoom();
    }

    public Room GetCurrentRoom(){
        return currentRoom;
    }

    public RoomCoordinate GetCurrentRoomCoordinate(){
        return currentRoom.GetRoomCoordinate();
    }

    public void SetCurrentRoom(Room room){
        currentRoom = room;
    }
}
