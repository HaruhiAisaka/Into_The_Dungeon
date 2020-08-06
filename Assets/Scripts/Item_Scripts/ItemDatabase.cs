using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    #region Non-Equipable-Items
    
    [SerializeField] private Key redKey, blueKey, greenKey, yellowKey, purpleKey = new Key();
    [SerializeField] private Map map = new Map();
    [SerializeField] private Compass compass = new Compass();
    [SerializeField] private Heart heart = new Heart();
    [SerializeField] private Armor leatherArmor, chainArmor, legendaryArmor = new Armor();
    #endregion

    #region Equipable-Items
    [SerializeField] private Boomberang boomberang, legendaryBoomberang = new Boomberang();
    [SerializeField] private Instrument piccolo = new Instrument();
    [SerializeField] private ProjectileWeapon bow = new ProjectileWeapon();
    [SerializeField] private Sword woodenSword, whiteSword , legendarySword = new Sword();

    #endregion


    // All items are organized into these two dictionaries.
    // One references items by name. The other by id number.
    private Dictionary<string, Item> itemDatabaseByString = new Dictionary<string, Item>();
    private Dictionary<int, Item> itemDatabaseByID = new Dictionary<int, Item>();

    // Seperate Dictionary for Keys.
    // All keys can also be accessed through the items database.
    private Dictionary<Door.LockedDoorColor, Key> keyDatabase = new Dictionary<Door.LockedDoorColor, Key>();

    public Item GetItem(string itemName){
        return itemDatabaseByString[itemName];
    }

    public Item GetItem(int itemID){
        return itemDatabaseByID[itemID];
    }

    public Key GetKey(Door.LockedDoorColor color){
        return keyDatabase[color];
    }

    private void AddItem(Item item){
        itemDatabaseByString.Add(item.name, item);
        itemDatabaseByID.Add(item.itemID, item);
        if (item is Key){
            Key key = (Key) item;
            keyDatabase.Add(key.color, key);
        }
    }

    void Awake() {
        AddItem(redKey);
        AddItem(blueKey);
        AddItem(greenKey);
        AddItem(yellowKey);
        AddItem(purpleKey);
    }
}
