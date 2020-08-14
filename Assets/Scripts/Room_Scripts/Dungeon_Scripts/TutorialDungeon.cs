using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialDungeon : Dungeon
{
    private void Start() {
        GenerateTutorialDungeon();
        dungeonDisplay.CreateDungeon(this);
    }

    private void GenerateTutorialDungeon(){
        endRoom = new Room(0,2, endRoom : true);
        AddRoom(startRoom);
        AddRoom(new Room(-1,0));
        AddRoom(new Room(1,0));
        AddRoom(new Room(0,1));
        AddRoom(endRoom);
        roomConnectors.Add(
            new Door(GetRoom(-1,0),GetRoom(0,0),Door.DoorState.open));
        roomConnectors.Add(
            new Door(GetRoom(1,0),GetRoom(0,0),Door.DoorState.open));
        roomConnectors.Add(
            new Door(GetRoom(0,1),GetRoom(0,0),Door.DoorState.locked, 
            Door.LockedDoorColor.red)
        );
        roomConnectors.Add(
            new Stair(GetRoom(0,1),GetRoom(-1,0), 
            new Vector2(.5f,.5f), new Vector2(.5f,.5f))
        );
        roomConnectors.Add(
            new Door(GetRoom(0,1),GetRoom(0,2),Door.DoorState.open)
        );
        startRoom.AddItem(itemDatabase.GetItem("Red Key"), new Vector2(3,0));
    }
}
