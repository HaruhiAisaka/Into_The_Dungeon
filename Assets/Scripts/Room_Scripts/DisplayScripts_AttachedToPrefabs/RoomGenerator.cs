﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class RoomGenerator : MonoBehaviour
{
    private RoomCoordinate roomCoordinate;
    private Room room;
    [SerializeField] private Chalice chalice;
    [SerializeField] private DoorDisplay[] doorDisplays;
    [SerializeField] private StairDisplay stairDisplayPrefab;
    private List<StairDisplay> stairDisplays =
        new List<StairDisplay>();

    [SerializeField] private GameObject items;
    [SerializeField] private ItemDrop itemDropPrefab;

    private Player player;

    public void GenerateRoom(Room room)
    {
        player = FindObjectOfType<Player>();
        this.room = room;
        roomCoordinate = room.roomCoordinate;
        this.transform.position = roomCoordinate.GetRoomWorldPosition();
        // Spawns the chalice if it is the end room
        if (room.endRoom == true)
        {
            Instantiate(chalice, this.transform);
            chalice.transform.localPosition = new Vector2(0, 0);
        }
        GenerateRoomConnections();
        GenerateItems();
        GenerateEnemies();
    }

    #region roomConnector Generation
    private void GenerateRoomConnections()
    {
        foreach (RoomConnector connector in room.roomConnectors)
        {
            if (connector is Door)
            {
                GenerateDoor((Door)connector);
            }
            else if (connector is Stair)
            {
                GenerateStairs((Stair)connector);
            }
        }
    }
    private void GenerateDoor(Door door)
    {
        Room nextRoom = door.GetNextRoom(room);
        Vector2 vectorBetweenRooms =
            nextRoom.roomCoordinate.GetVector2() -
            room.roomCoordinate.GetVector2();
        Cardinal4.Direction direction =
            Cardinal4.Vector2ToDirection(vectorBetweenRooms);
        DoorDisplay doorDisplay =
            Array.Find(doorDisplays,
            element => element.direction == direction);
        doorDisplay.door = door;
        door.doorDisplay = doorDisplay;
    }

    private void GenerateStairs(Stair stair)
    {
        StairDisplay newStair = Instantiate(stairDisplayPrefab, this.transform);
        newStair.transform.localPosition = (Vector3)stair.GetStairPosition(room);
        newStair.stair = stair;

        stairDisplays.Add(newStair);
    }

    // Activates or deactivates the hit boxes that trigger door animations.
    public void EnableDoorAnimations(bool enable)
    {
        foreach (DoorDisplay doorDisplay in doorDisplays)
        {
            doorDisplay.EnableAnimations(enable);
        }
    }

    // Activates or deactivates the hit boxes that trigger stair animations.
    public void EnableStairAnimations(bool enable)
    {
        foreach (StairDisplay stairDisplay in stairDisplays)
        {
            stairDisplay.EnableAnimations(enable);
        }
    }
    #endregion

    #region Item Generation
    private void GenerateItems()
    {
        foreach (Item item in room.items)
        {
            ItemDrop newItemDrop = Instantiate(itemDropPrefab, items.transform);
            newItemDrop.transform.localPosition = item.localPosition;
            newItemDrop.item = item;
        }
    }

    #endregion

    #region Enemy Generation

    private void GenerateEnemies()
    {
        foreach (GameObject enemyPrefab in room.enemies)
        {
            GameObject enemy = Instantiate(enemyPrefab, transform);
        }

    }
    #endregion

}
