# Rooms Doc
How Rooms are currently implemented in the game.

## RoomCoordinate
RoomCoordinate is a public class which represents the coordinates of a room in the game. 
Instantiate a RoomCoordinate object by typing RoomCoordinate(x,y) x y being the coordinates of the Room. The RoomCoordinate of a room is relative to the starting room which is always at RoomCoordinate (0,0). So a Room to the left of (0,0) will have RoomCoordinate (-1,0). A room to the left of (-1,0) will have RoomCoordinate (-2,0).
You can get a Vector2 version of the RoomCoordinate by using GetRoomCoordinateVector2().

## Room
Room is a custom class that defines what a single room in a dungeon is. 
Currently it only stores the RoomCoordinate of the Room along with it's adjacent Rooms if it has any. **However, Room will eventually store all information that partains to the room.** This includes the enemies in the room, items, doors, staircases, moveable blocks, and anything else that is needed to create the room.

## Dungeon
The Dungeon stores all the Rooms in a dungeon. There should only be one instance of this class at a time.