using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosedDoorHitBox : MonoBehaviour
{   
    [SerializeField] private Door door;
    //Method used for when the player attempts to run into a door to unlock it.
    private void OnCollisionStay2D(Collision2D other) {
        door.CollisionStay(other);
    }
    private void OnCollisionExit2D(Collision2D other) {
        door.CollisionExit(other);
    }
}
