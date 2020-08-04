using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stair : RoomConnector
{   
    // x and y are the position the stair is located relative to the center of the room.

    public Vector2 stairPosition1;
    public Vector2 stairPosition2;
    
    public const float squareSize = 1;


    public Stair (
        Room room1, Room room2,
        Vector2 stairPosition1, Vector2 stairPosition2
    ) : base (room1, room2)
    {
        // Assurtions
        if (!Room.IsInRoom(stairPosition1, squareSize, squareSize)||
            !Room.IsInRoom(stairPosition2, squareSize, squareSize))
        {
            throw new System.ArgumentException ("Stairs must fit inside of the room");
        }
        this.stairPosition1 = stairPosition1;
        this.stairPosition2 = stairPosition2;
    }


    // Get the stair position relative to the center of the room
    public Vector2 GetStairPosition(Room room){
        if (room == base.room1) return stairPosition1;
        else return stairPosition2;
    }

    // Get the stair position relative to the world origin/scene
    public Vector2 GetStairWorldPosition(Room room){
        return GetStairPosition(room) + room.roomCoordinate.GetRoomWorldPosition();
    }
}
