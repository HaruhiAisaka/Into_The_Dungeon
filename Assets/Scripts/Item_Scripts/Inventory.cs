using System.Collections;
using System.Collections.Generic;
using UnityEngine;

private class Inventory {
    private List<Item> equipInventory = new List<EquipableItem>();
    private List<Item> nonequipInventory = new List<NonEquipableItem>();
    private List<Item> keyInventory = new List<Key>();

    private delegate bool SearchCriteria(Item item); // Used in GetItem for searches

    public void Add(Item item) {
        switch (item) {
            case EquipableItem item:
                equipInventory.Add(item);
                break;
            case NonEquipableItem item:
                nonequipInventory.Add(item);
                break;
            case Key key:
                keyInventory.Add(key);
                break;
        }
    }

    public Item GetItem(string itemName) {
        return GetItem(item => item.itemName == itemName);
    }

    public Item GetItem(int itemID) {
        return GetItem(item => item.itemID == itemID);
    }

    /** Returns the first item that matches the searchCriteria, and null otherwise  */
    private Item GetItem(SearchCriteria searchCriteria) {
        foreach (Item item : equipInventory) {
            if (searchCriteria(item)) {
                return item;
            }
        }
        foreach (Item item : nonequipInventory) {
            if (searchCriteria) {
                return item;
            }
        }
        foreach (Item item : keyInventory) {
            if (searchCriteria) {
                return item;
            }
        }

        return null;
    }

    public bool InInventory(string itemName) {
        return
            keyInventory.Exists(item => item.name == itemName) ||
            nonequipInventory.Exists(item => item.name == itemName) ||
            equipInventory.Exists(item => item.name == itemName);
    }

    public bool InInventory(int itemId) {
        return
            keyInventory.Exists(item => item.itemId == itemId) ||
            nonequipInventory.Exists(item => item.itemId == itemId) ||
            equipInventory.Exists(item => item.itemId == itemId)
    }

    public bool KeyInInventory(Door.LockedDoorColor color) {
        return keyInventory.Exists(key => key.color == color);
    }

    public List<EquipableItem> GetEquipableItems() {
        return equipInventory;
    }

    public List<NonEquipableItem> GetNonEquipableItems() {
        return nonequipInventory;
    }

    public List<Key> GetKeys() {
        return keyInventory;
    }
}
