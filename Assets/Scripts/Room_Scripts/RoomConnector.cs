
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

        room1.roomConnectors.Add(this);
        room2.roomConnectors.Add(this);
    }

    public Room GetNextRoom(Room currentRoom){
        if (currentRoom == room1) return room2;
        else return room1;
    }
}