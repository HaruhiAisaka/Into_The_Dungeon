using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Room
{
    [SerializeField] private RoomCoordinate roomCoordinate;
    private Room northRoom;
    private Room southRoom;
    private Room eastRoom;
    private Room westRoom;

    public Room(int x, int y,
        Room northRoom = null,
        Room southRoom = null,
        Room eastRoom = null,
        Room westRoom = null)
    {
        this.roomCoordinate = new RoomCoordinate(x,y);
        this.northRoom = northRoom;
        if (northRoom != null) northRoom.southRoom = this;
        this.southRoom = southRoom;
        if (southRoom != null) southRoom.northRoom = this;
        this.eastRoom = eastRoom;
        if (eastRoom != null) eastRoom.westRoom = this;
        this.westRoom = westRoom;
        if (westRoom != null) westRoom.eastRoom = this;
    }

    public RoomCoordinate GetRoomCoordinate(){
        return roomCoordinate;
    }

    public Room GetAdjacentRoom (CardinalDirection.Direction4 direction){
        switch (direction){
            case CardinalDirection.Direction4.EAST:
                return this.eastRoom;
            case CardinalDirection.Direction4.NORTH:
                return this.northRoom;
            case CardinalDirection.Direction4.WEST:
                return this.westRoom;
            case CardinalDirection.Direction4.SOUTH:
                return this.southRoom;
            case CardinalDirection.Direction4.ZERO_VECTOR:
                throw new System.ArgumentException("direction can not be ZERO_VECTOR");
            default:
                throw new System.ArgumentException("Invalid entry");
        }
    }


}
