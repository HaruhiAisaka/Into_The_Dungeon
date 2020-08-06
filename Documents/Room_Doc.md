# Rooms Doc
How Rooms and RoomConnections are currently implemented in the game.

## Custom Classes vs. Display Classes
The classes for the implimentation of Rooms is seperated into two catagories: **Custom Classes** and **Display Classes**

**Custom Classes** are classes that are not attached to any gameObject in Unity. They are responsible for storing information reguarding the creation of room objects.

**Display Classes** are classes that are attacked to gameObjects in Unity. They are responsible for displaying the components of a Room given the custom class that it is using to create it.

For example, a Room has two classes associated with it. The "Room" class which stores information about the room, and the "Room Generator" class which generates a room given the information in the "Room" class.

|CustomClass|DisplayClass|
|-----------|------------|
|Room|RoomGenerator|
|Door|DoorDisplay|
|Stair|StairDisplay|
|Item|ItemDrop|

## RoomCoordinate (CustomClass)
RoomCoordinate is a public class which represents the coordinates of a room in the game. 
Instantiate a RoomCoordinate object by typing RoomCoordinate(x,y) x y being the coordinates of the Room. The RoomCoordinate of a room is relative to the starting room which is always at RoomCoordinate (0,0). So a Room to the left of (0,0) will have RoomCoordinate (-1,0). A room to the left of (-1,0) will have RoomCoordinate (-2,0).
You can get a Vector2 version of the RoomCoordinate by using GetRoomCoordinateVector2().

## Room (CustomClass)
Room is a custom class that defines what a single room in a dungeon is. 
Currently it only stores the RoomCoordinate of the Room along with any room connectors if it has any. **However, Room will eventually store all information that partains to the room.** This includes the enemies in the room, items, doors, staircases, moveable blocks, and anything else that is needed to create the room.

**Note** Now, connections between rooms are **not** specified during the instantiation of a room. Instead, they are specified when a RoomConnector object is instantiated.

## Room Generator (Display Class)
The RoomGenerator generates a given room in the scene when it is instantiated in the dungeon class. It uses all the information provided by the received Room instance in order to create the room.

*Currently, the way to instantate a room is to call InstantiateRoom(Room room) in the dungeon class.* This is a public method.

## RoomConnector (CustomClass)
Is the base class for all objects that connect two rooms together. Doors and stairs inheret from this class. The RoomConnector when instantiated requires two room instances. 
GetNextRoom(Room room) gives the "next room" of the RoomConnector given that the current room is inputed as a parameter.

When a RoomConnector is instantiated, it will automatically add itself to a list of RoomConnectors that are stored seperatley in the two rooms that the RoomConnector connects.

## Doors (CustomClass)
Doors inherets from RoomConnector. It also stores the door state and the color key needed to unlock the door. Door objects should be instantiated in the dungeon class in order for them to be stored in a list of RoomConnectors stored in Dungeon.

## DoorDisplay (DisplayClass)
Displays a door that can be used in a room given a "Door" instance. 
**Note** currently there are always four Door Display objects in a room. If a Door Display receives *null* for a door, it displays nothing but still keeps it's collider active. 
This is done so that RoomGenerator doesn't have to store multiple colliders that represent the walls of a room given the number of doors.

## Stair (CustomClass)
Inherets from RoomConnector. It also stores the position of the stair relative to the center of the room.
**Note** I might make an interface for all possible gameObjects that need to store their initial positions in a room. For example, if Custom Enemy class is created, they will need to store the position in the room where they will spawn. I might make an interface for all those gameObjects.

## StairDisplay (DisplayClass)
Displays a stair that can be used in a room given a "Stair" instance. These stairs are created/instantiated in a room when the room is generated, allowing for multiple stairs in a single room.


## Dungeon
The Dungeon stores all the Rooms in a dungeon. There should only be one instance of this class at a time.

The Dungeon is also responsible for the generation of rooms. The dungeon will always generate the starting room when the scene is first entered.


## Room Creation and Deletion
In a sense the dungeon is the class that connects the Custom classes and the Display classes. The Dungeon stores information of a dungeon by creating new instances of Room/Door/and StairClasses. The Dungeon then creates these rooms using the RoomGenerator class, feeding into the RoomGenerator class the Room that it wants created.
![How_Dungeon_Connects_Everything](RoomDoc_Images/Room_Classes.PNG)

When the player enters a Display object that represents a RoomConnector (such as a DoorDisplay or a StairDisplay), the Dungeon class is called to create the next Room and, once all animations are completed, the exited RoomGenerator is destroyed.
This way, there will always be only one Room generated at a time, while having information reguarding all rooms stored in the Room classes stored in Dungeon.

TLDR:
When player uses a door or stairway:
1. Display Class gets the nextRoom from RoomConnector instance stored in the Display Class
2. Calls on Dungeon to generate the nextRoom.
3. Performs all nessecary animations.
4. Deletes the original room from the scene.

![Room_Creation_Room_Deletion](RoomDoc_Images/Room_Connector_Trigger.PNG)