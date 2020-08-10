using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sword : EquipableItem
{
    public int damage;

    // range = the length of the swordHitBox. Aka the range of the sword.
    public float range;

    public SwordHitBox sowrdHitBox;

    public override void OnEquip(PlayerInventory playerInventory){
        
    }

    public override void Action(PlayerInventory playerInventory){

    }

    public override void OnUnEquip(PlayerInventory playerInventory){
    
    }

    public override void OnPickUp(PlayerInventory playerInventory){
        
    }

}