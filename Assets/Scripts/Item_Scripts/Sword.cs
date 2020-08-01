using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sword : EquipableItem
{
    public int damage;

    // range = the length of the swordHitBox. Aka the range of the sword.
    public float range;

    // tier defines the supiriority of a sword compared to other swords. 
    // 0 being the least powerful and 3 being the most. 
    // When the player picks up a higher level sword, 
    // it disgards the lower tier sword from inventory
    public int teir;

    public SwordHitBox sowrdHitBox;

    public override void OnEquip(){
        
    }

    public override void Action(){

    }

    public override void SecondaryAction(){

    }

    public override void OnUnEquip(){

    }
}