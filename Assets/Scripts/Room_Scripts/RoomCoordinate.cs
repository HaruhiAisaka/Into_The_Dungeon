using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomCoordinate
{
    /*The room coordinate records the current room the player is in 
    relative to the original room.
    The player always starts at room (0,0). 
    A room to the left of (0,0) is (-1,0), to the right is (0,1) and so on.
    To find the real world position of the center of the room the player is in,
    call the getRealRoomCoordinate() function. 
    The center of the origin room must be (0,0)*/
    public Vector2 roomCoordinate{get; private set;}

    public int x {get; private set;}

    public int y {get; private set;}

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

    public RoomCoordinate(int x, int y){
        this.x = x;
        this.y = y;
        this.roomCoordinate = new Vector2 (x,y);
    }

    public Vector2 GetRoomCoordinateVector2() {
        return roomCoordinate;
    }

    public Vector2 GetRoomWorldPosition(){
        return new Vector2(roomCoordinate.x * xFactor, roomCoordinate.y * yFactor);
    }
}

public class RoomCoordinateEqual : EqualityComparer<RoomCoordinate>{
    public override bool Equals(RoomCoordinate r1, RoomCoordinate r2){
        if (r1 == null && r2 == null){
            return true;
        }
        else if (r1 == null || r2 == null){
            return false;
        }
        return (r1.x == r2.x && r1.y == r2.y);
    }

    public override int GetHashCode(RoomCoordinate r){
        int hCode = r.x ^ r.y;
        return hCode.GetHashCode();
    }
}