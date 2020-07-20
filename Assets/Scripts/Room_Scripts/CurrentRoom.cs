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
    /*The room coordinate records the current room the player is in 
    relative to the original room.
    The player always starts at room (0,0). 
    A room to the left of (0,0) is (-1,0), to the right is (0,1) and so on.
    To find the real world position of the center of the room the player is in,
    call the getRealRoomCoordinate() function. 
    The center of the origin room must be (0,0)*/
    public Vector2 roomCoordinate;

    /* The xFactor is the distance between the centers of two rooms 
    in the horizontal direction. 
    For example, if the current room is positioned at (0,0), 
    then the center of the room on the left is positioned at (-xFactor,0).*/
    private int xFactor = 18;
    /* The yFactor is the distance between the centers of two rooms 
    in the vertical direction. 
    For example, if the current room is positioned at (0,0), 
    then the center of the room above is positioned at (0,yFactor).*/
    private int yFactor = 10;

    private Player player;
    
    // Start is called before the first frame update
    void Start()
    {
        roomCoordinate = new Vector2(0,0);
        player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Functions that deal with setting and getting the room cordinate.
    public Vector2 GetRoomCoordinate(){
        return roomCoordinate;
    }

    public Vector2 GetRealRoomCoordinate(){
        return new Vector2 (roomCoordinate.x * xFactor, roomCoordinate.y * yFactor);
    }

    /* UpdateRoomByDoor() updates the room the player is in when the player 
    goes to another room through a door. */
    public void UpdateRoomByDoor(){
        var playerPosition = player.transform.position;
        float xPlayerDistanceFromCenter = playerPosition.x / xFactor;
        float yPlayerDistanceFromCenter = playerPosition.y / yFactor;
        if (Mathf.Abs(xPlayerDistanceFromCenter) > Mathf.Abs(yPlayerDistanceFromCenter)){
            if (xPlayerDistanceFromCenter < roomCoordinate.x){
                roomCoordinate.x --;
            } 
            else {
                roomCoordinate.x ++;
            }
        }
        else {
            if (yPlayerDistanceFromCenter < roomCoordinate.y){
                roomCoordinate.y --;
            }
            else {
                roomCoordinate.y ++;
            }
        }
    }

    public int GetXFactor(){
        return xFactor;
    }

    public int GetYFactor(){
        return yFactor;
    }
}
