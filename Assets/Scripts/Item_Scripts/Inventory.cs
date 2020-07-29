using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Inventory {
    private List<EquipableItem> equipInventory = new List<EquipableItem>();
    private List<NonEquipableItem> nonequipInventory = new List<NonEquipableItem>();
    private List<Key> keyInventory = new List<Key>();

    private delegate bool SearchCriteria(Item item); // Used in GetItem for searches

    public void Add(Item item) {
        switch (item) {
            case EquipableItem equipable:
                equipInventory.Add(equipable);
                break;
            case Key key:
                keyInventory.Add(key);
                break;
            case NonEquipableItem nonEquipable:
                nonequipInventory.Add(nonEquipable);
                break;
        }
    }

    public Item Remove(string itemName) {
        return Remove(item => item.name == itemName);
    }

    public Item Remove(int itemID) {
        return Remove(item => item.itemID == itemID);
    }

    private Item Remove(SearchCriteria searchCriteria) {
        for (int i = 0; i < equipInventory.Count; i++) {
            if (searchCriteria(equipInventory[i])) {
                Item item = equipInventory[i];
                equipInventory.RemoveAt(i);
                return item;
            }
        }
        for (int i = 0; i < nonequipInventory.Count; i++) {
            if (searchCriteria(nonequipInventory[i])) {
                Item item = nonequipInventory[i];
                nonequipInventory.RemoveAt(i);
                return item;
            }
        }

        for (int i = 0; i < keyInventory.Count; i++) {
            if (searchCriteria(keyInventory[i])) {
                Item item = keyInventory[i];
                keyInventory.RemoveAt(i);
                return item;
            }
        }

        return null;
    }

    public Item GetItem(string itemName) {
        return GetItem(item => item.name == itemName);
    }

    public Item GetItem(int itemID) {
        return GetItem(item => item.itemID == itemID);
    }

    /** Returns the first item that matches the searchCriteria, and null otherwise  */
    private Item GetItem(SearchCriteria searchCriteria) {
        foreach (Item item in equipInventory) {
            if (searchCriteria(item)) {
                return item;
            }
        }
        foreach (Item item in nonequipInventory) {
            if (searchCriteria(item)) {
                return item;
            }
        }
        foreach (Item item in keyInventory) {
            if (searchCriteria(item)) {
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

    public bool InInventory(int itemID) {
        return
            keyInventory.Exists(item => item.itemID == itemID) ||
            nonequipInventory.Exists(item => item.itemID == itemID) ||
            equipInventory.Exists(item => item.itemID == itemID);
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
