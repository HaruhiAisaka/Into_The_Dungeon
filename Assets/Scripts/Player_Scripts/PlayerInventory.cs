using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    private ItemDatabase itemDatabase;
    public PlayerUI playerUI {get; protected set;}
    public Inventory inventory {get; protected set;} = new Inventory();

    private void Start() {
        itemDatabase = FindObjectOfType<ItemDatabase>();
        playerUI = FindObjectOfType<PlayerUI>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.GetComponent<ItemDrop>() is ItemDrop itemDrop) {
            PickUpItem(itemDrop);
            Object.Destroy(itemDrop.gameObject);
        }
    }

    private void PickUpItem(ItemDrop drop) {
        if (drop != null && drop.GetItem() is Item item){
            inventory.Add(item);
            item.OnPickUp(this);
        }
    }

    public void Add(Item item) {
        inventory.Add(item);
    }

    public Item Remove(string itemName) {
        return inventory.Remove(itemName);
    }

    public Item Remove(int itemID) {
        return inventory.Remove(itemID);
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
