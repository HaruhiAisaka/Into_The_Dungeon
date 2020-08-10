using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Key : NonEquipableItem
{
    public Door.LockedDoorColor color;

    public override void OnPickUp(PlayerInventory playerInventory){
        playerInventory.playerUI.UpdateKeys(playerInventory.inventory.GetKeys());
    }
}
