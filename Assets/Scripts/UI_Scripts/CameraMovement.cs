using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public void SetCameraToNewRoom(Room room){
        Vector3 newCenter = room.roomCoordinate.GetRoomWorldPosition();
        newCenter.z = this.transform.position.z;
        this.transform.position = newCenter;
    }

    public IEnumerator MoveCameraToNewRoom(Room room, float totalTime){
        Vector3 initialCameraPosition = this.transform.position;
        Vector3 newCenter = room.roomCoordinate.GetRoomWorldPosition();
        newCenter.z = this.transform.position.z;
        float t = 0;
        while (Vector3.Distance(this.transform.position, newCenter)>0){
            // player.FreezePlayer();
            t += Time.deltaTime;
            this.transform.position = Vector3.Lerp(initialCameraPosition, newCenter, t / totalTime);
            yield return null;
        }
    }
}
