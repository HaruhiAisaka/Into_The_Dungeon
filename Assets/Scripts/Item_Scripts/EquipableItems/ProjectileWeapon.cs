using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ProjectileWeapon : EquipableItem 
{
    public GameObject projectile;
    public override void OnEquip(PlayerInventory playerInventory){
        
    }

    public override void Action(PlayerInventory playerInventory){

    }

    public override void OnUnEquip(PlayerInventory playerInventory){
    
    }

    public override void OnPickUp(PlayerInventory playerInventory){
        
    }
}