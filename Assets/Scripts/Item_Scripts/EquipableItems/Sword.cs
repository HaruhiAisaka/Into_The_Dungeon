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

    public override void OnEquip(){
        
    }

    public override void Action(){

    }

    public override void OnUnEquip(){

    }
}