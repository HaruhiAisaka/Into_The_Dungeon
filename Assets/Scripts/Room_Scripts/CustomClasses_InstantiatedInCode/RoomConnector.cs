
public abstract class RoomConnector{
    public Room room1 {get; private set;}
    public Room room2 {get; private set;}

    // Creates a connection between two rooms. 
    // All RoomConnectors (doors and stairs) uses this constructor.
    // room1 and room2 must be an existing room that
    // exists in the dungeon.
    public RoomConnector (Room room1, Room room2){
        //Assertions 

        if (room1 == null || room2 == null){
            throw new System.ArgumentNullException
                ("Inputed rooms can not be null");
        }
        this.room1 = room1;
        this.room2 = room2;

        room1.AddRoomConnector(this);
        room2.AddRoomConnector(this);
    }

    public Room GetNextRoom(Room currentRoom){
        if (currentRoom == room1) return room2;
        else return room1;
    }

    public bool HasRoom(Room room){
        bool inRoom1 = (room1 == room);
        bool inRoom2 = (room2 == room);
        return inRoom1 || inRoom2;
    }

    public bool HasRoom(RoomCoordinate roomCoordinate){
        bool inRoom1 = RoomCoordinate.Equals(roomCoordinate, room1.roomCoordinate);
        bool inRoom2 = RoomCoordinate.Equals(roomCoordinate, room2.roomCoordinate);
        return inRoom1 || inRoom2;
    }
}