using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Room
{
    [SerializeField] private RoomCoordinate roomCoordinate;
    private Room northRoomByDoor;
    private Room southRoomByDoor;
    private Room eastRoomByDoor;
    private Room westRoomByDoor;

    public Room(int x, int y, 
        Room northRoomByDoor = null, 
        Room southRoomByDoor = null, 
        Room eastRoomByDoor = null, 
        Room westRoomByDoor = null)
    {
        this.roomCoordinate = new RoomCoordinate(x,y);
        this.northRoomByDoor = northRoomByDoor;
        if (northRoomByDoor != null) northRoomByDoor.southRoomByDoor = this;
        this.southRoomByDoor = southRoomByDoor;
        if (southRoomByDoor != null) southRoomByDoor.northRoomByDoor = this;
        this.eastRoomByDoor = eastRoomByDoor;
        if (eastRoomByDoor != null) eastRoomByDoor.westRoomByDoor = this;
        this.westRoomByDoor = westRoomByDoor;
        if (westRoomByDoor != null) westRoomByDoor.eastRoomByDoor = this;
    }

    public RoomCoordinate GetRoomCoordinate(){
        return roomCoordinate;
    }

    public Room GetAdjacentRoom (CardinalDirection.Direction4 direction){
        switch (direction){
            case CardinalDirection.Direction4.EAST:
                return this.eastRoomByDoor;
            case CardinalDirection.Direction4.NORTH:
                return this.northRoomByDoor;
            case CardinalDirection.Direction4.WEST:
                return this.westRoomByDoor;
            case CardinalDirection.Direction4.SOUTH:
                return this.southRoomByDoor;
            case CardinalDirection.Direction4.ZERO_VECTOR:
                throw new System.ArgumentException("direction can not be ZERO_VECTOR");
            default:
                throw new System.ArgumentException("Invalid entry");
        }
    }

    
}
