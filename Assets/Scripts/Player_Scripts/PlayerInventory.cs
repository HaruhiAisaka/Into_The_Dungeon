using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    private ItemDatabase itemDatabase;
    private List<Item> inventory = new List<Item>();

    private void Start() {
        itemDatabase = FindObjectOfType<ItemDatabase>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.GetComponent<ItemDrop>()){
            Item item = other.GetComponent<ItemDrop>().GetItem();
            inventory.Add(item);
            Object.Destroy(other.gameObject);
        }
    }

    public bool InInventory(string itemName){
        Item item = itemDatabase.GetItem(itemName);
        return inventory.Contains(item);
    }

    public bool InInventory(int itemID){
        Item item = itemDatabase.GetItem(itemID);
        return inventory.Contains(item);
    }

    public bool KeyInInventory(Door.LockedDoorColor color){
        Key key = itemDatabase.GetKey(color);
        return inventory.Contains(key);
    }

    
}
