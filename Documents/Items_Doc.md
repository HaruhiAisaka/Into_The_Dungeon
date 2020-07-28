# Items Doc
How items currently function in the game.

## Item Classes
All items are defined by a custom class in unity called Item. 
Item is an **abstract class** which **does not inherit monobehavior**. 
This means that the script or scripts that inherit from Item cannot be attatched to a game object as a component.

All items that are in the game are stored in a gameObject called ItemDatabase. All gameObjects and classes can use ItemDatabase to search for an item and receive the item. The ItemDatabase will be specified latter.

**Item Class Fields**
| Field|Type|Representation|
|------|----|--------------|
|name|string|Name of the item. Can be used to search for the item in the ItemDatabase|
|itemID|int|ID of the item. Can be used to search for the item in the ItemDatabase|
|description|string|Description of the item
|sprite|Sprite|Sprite used for the item. The sprite is intended to be used as a menu icon or as an icon when the item is droped in the room.

Items are further divided into two classes: *EquipableItem* and *NonEquipableItem*.
Equipable items are items that can be equiped by the player. The player can choose the item and press a button to perform an action using that item. 
Equipable items are items such as, swords, bombs, bows, magical wands, musical instruments, basically any item that requires the player to equip it and then requires the player to perform an action to use it.
Non-equipable items are items that can not be equiped by the player. The item does not require the player to press an action button to use an item.
Non-equipable items are items such as: keys, maps, compases, extra hearts to replenish health, anything that doesn't require the player to manually equip it to use.

Currently, these two items are empty, but I presume that they will eventually be filled with fields and methods unique to those classes.

The classes that represent specific items should then be specificed further, inhereting from those two classes. Currently, the only specified item that has it's own class is Keys. Keys is inhereted from the class NonEquipableItem.

Again, **Non of the classes bellow inheret monobehavior** so they can not be directly attatched to gameObjects as components.
![Item Classes](ItemDoc_Images/Item_Class_Structure.png)

## ItemDatabase
The item database stores all items that exist in the game. All items are initialized in this class. Items can be initialized in the inspector once a variable in the class that is assigned to the item is declared. For example, in order to initialize a Red Key, a variable redKey is created in the class ItemDatabase and then the fields of the Item can be specified in the Inspector.The item must then be put into the method AddItem(item) in the Awake() method. AddItem is a private method that adds the newly created item into appropreate dictionaries in ItemDatabase.

The reason why it's important for items to be instantiated in the inspector is because it's easier to assign sprites in the sprite field of an Item by draging it from the assets folder into the inspector instead of hard coding it through code. This is the same for Animators and other unity objects that may need to be used in the future.

Item Database has a main public function, GetItem(itemName) or GetItem(itemID), that returns an Item given the name or the itemID of an item. There is also another public function that returns a Key given a color.

## PlayerInventory
Player now has another script attatched to it called PlayerInventory. PlayerInventory is mainly a list of Items that represents items that the player has. 
The OnTriggerEnter2D function picks up items that are droped in the dungeon. 
There is also a mehtod InInventory(itemName) or InInventory(itemID) that returns a bool wether or not the item is in the player inventory. 
There is also a method that searches if a specific key is in the inventory KeyInInventory(Door.LockedDoorColor color).

## ItemDrop
Item Drop is used in the prefab, ItemDrop which is used whenever any item is dropped into the dungeon. For example, a key can be lying on the floor in the dungon. All you need to do is type the name *or* ID of the item into the appropreate fields, and (if the item was properly instanciated in ItemDatabase) the item will appear on the floor of the dungeon.
Whenever a player comes in contact with the Item Drop gameObject, the gameObject is destroyed and the Item is added into the player inventory.

![How it all comes together](ItemDoc_Images/All_Item_Classes.png)