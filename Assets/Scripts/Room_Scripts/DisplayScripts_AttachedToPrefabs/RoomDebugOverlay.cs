using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomDebugOverlay : MonoBehaviour
{
    [SerializeField] private RoomGenerator roomGenerator;
    [SerializeField] private SpriteRenderer clusterOverlay;
    private Room room;

    [SerializeField] private int clusterNum;

    private void Start() {
        room = roomGenerator.room;
        Color newColor = Color.HSVToRGB(0 + (.1f*room.cluster), .5f,.9f);
        newColor.a = .6f;
        clusterOverlay.color = newColor;
        clusterNum = room.cluster;
    }

    [ContextMenu("Print ClusterNum and Room Coordinate")]
    public void PrintClusterNumAndRoomCoordinate(){
        Debug.Log("ClusterNum: " + clusterNum);
        Debug.Log("Room Coordinate: " + room.roomCoordinate.GetVector2());
    }
}
