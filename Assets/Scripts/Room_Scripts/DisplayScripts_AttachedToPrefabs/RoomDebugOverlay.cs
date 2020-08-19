using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomDebugOverlay : MonoBehaviour
{
    [SerializeField] private RoomGenerator roomGenerator;
    [SerializeField] private SpriteRenderer clusterOverlay;
    private Room room;

    private void Start() {
        room = roomGenerator.room;
        Color newColor = Color.HSVToRGB(0 + (.1f*room.cluster), .5f,.9f);
        newColor.a = .6f;
        clusterOverlay.color = newColor;
    }
}
