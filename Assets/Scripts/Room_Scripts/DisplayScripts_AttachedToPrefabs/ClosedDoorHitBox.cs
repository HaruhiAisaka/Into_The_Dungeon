﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosedDoorHitBox : MonoBehaviour
{   
    [SerializeField] private DoorDisplay doorDisplay;
    //Method used for when the player attempts to run into a door to unlock it.
    private void OnCollisionStay2D(Collision2D other) {
        doorDisplay.CollisionStay(other);
    }
    private void OnCollisionExit2D(Collision2D other) {
        doorDisplay.CollisionExit(other);
    }
}
