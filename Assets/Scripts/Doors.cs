using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doors : MonoBehaviour
{   
    private CurrentRoom currentRoom;
    private Player player;

    private Camera mainCamera;

    private void Start() {
        currentRoom = FindObjectOfType<CurrentRoom>();
        player = FindObjectOfType<Player>();
        mainCamera = FindObjectOfType<Camera>();
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.GetComponent<Player>()){
            currentRoom.UpdateRoomByDoor();
            var newRoomCenter = currentRoom.GetRealRoomCoordinate();
            player.transform.position = new Vector3 
                (newRoomCenter.x, 
                newRoomCenter.y, 
                player.transform.position.z);
            mainCamera.transform.position = new Vector3 
                (newRoomCenter.x, 
                newRoomCenter.y, 
                mainCamera.transform.position.z);
        }
    }
}
