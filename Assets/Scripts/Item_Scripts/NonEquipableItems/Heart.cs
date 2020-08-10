using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Heart : NonEquipableItem, IItemCopyable
{
    public int healthIncrease;

    public override void OnPickUp(PlayerInventory playerInventory){
        playerInventory.GetComponent<Player>().HealPlayer(healthIncrease);
    }

    public Item CopyItem(Vector2 newPosition){
        Heart newHeart = new Heart();
        newHeart.SetItemPosition(newPosition);
        return newHeart;
    }

}
