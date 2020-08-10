using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EquipableItem : Item
{
    // OnEquip() is triggered after the player equips the item 
    // and exits the pause screen.
    public abstract void OnEquip(PlayerInventory playerInventory);

    // Action should be triggered if the player has equiped this item and 
    // presses the "action" button.
    public abstract void Action(PlayerInventory playerInventory);

    // OnUnEquip() is triggered after the player unequips the item
    // and exits the pause screen.
    public abstract void OnUnEquip(PlayerInventory playerInventory);
    
}
