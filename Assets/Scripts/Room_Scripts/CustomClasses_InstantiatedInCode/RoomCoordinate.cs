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
    private float xFactor = 18;

    /* The yFactor is the distance between the centers of two rooms 
    in the vertical direction. 
    For example, if the current room is positioned at (0,0), 
    then the center of the room above is positioned at (0,yFactor).*/
    private float yFactor = 11;

    public RoomCoordinate(int x, int y){
        this.x = x;
        this.y = y;
        this.roomCoordinate = new Vector2 (x,y);
    }

    public RoomCoordinate(Vector2 vector2){
        this.x = (int) vector2.x;
        this.y = (int) vector2.y;
        this.roomCoordinate = vector2;
    }

    public Vector2 GetVector2() {
        return roomCoordinate;
    }

    public Vector2 GetRoomWorldPosition(){
        return new Vector2(roomCoordinate.x * xFactor, roomCoordinate.y * yFactor);
    }

    public bool Equals(RoomCoordinate r1){
        if (r1 == null){
            return false;
        }
        return (r1.x == roomCoordinate.x && r1.y == roomCoordinate.y);
    }

    public static bool Equals(RoomCoordinate r1, RoomCoordinate r2){
        if (r1 == null && r2 == null){
            return true;
        }
        else if (r1 == null || r2 == null){
            return false;
        }
        return (r1.x == r2.x && r1.y == r2.y);
    }

    // Returns a list of room coordinates that surround the current room coordinate.
    public static RoomCoordinate[] SurroundingsRoomCoordinates(RoomCoordinate roomCoordinate)
    {
        int x = roomCoordinate.x;
        int y = roomCoordinate.y;
        RoomCoordinate[] result =
            {new RoomCoordinate(x-1,y),
            new RoomCoordinate(x+1,y),
            new RoomCoordinate(x,y-1),
            new RoomCoordinate(x,y+1)};
        return result;
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