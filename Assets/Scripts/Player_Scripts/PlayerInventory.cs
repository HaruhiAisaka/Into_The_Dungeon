using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{

    private ItemDatabase itemDatabase;
    private PlayerUI playerUI;
    private Inventory inventory = new Inventory();

    private void Start() {
        itemDatabase = FindObjectOfType<ItemDatabase>();
        playerUI = FindObjectOfType<PlayerUI>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        ItemDrop itemDrop = other.GetComponent<ItemDrop>();
        if (itemDrop != null && itemDrop.GetItem() is Item item){
            inventory.Add(item);

            // If the item was a [key], we have to update the UI to reflect that
            if (item is Key key && playerUI != null) {
                playerUI.UpdateKeys(inventory.GetKeys());
            }

            Object.Destroy(itemDrop.gameObject);
        }
    }

    public bool InInventory(string itemName){
        return inventory.InInventory(itemName);
    }

    public bool InInventory(int itemID){
        return inventory.InInventory(itemID);
    }

    public bool KeyInInventory(Door.LockedDoorColor color){
        return inventory.KeyInInventory(color);
    }


}
