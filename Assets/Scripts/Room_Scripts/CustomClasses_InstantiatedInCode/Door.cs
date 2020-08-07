
public class Door : RoomConnector
{
    public DoorDisplay doorDisplay = null;
    public DoorState state {get; private set;}
    public LockedDoorColor color {get; private set;} = LockedDoorColor.none;
    // DoorState is the state of the door. 
    // There are currenlty 4 states though more will be added.
    // None means that there is no door, 
    // the sprite renderer is disabled and what is left is the doorCollider.
    public enum DoorState {open, closed, locked};
    public enum LockedDoorColor {none, red, blue, green, yellow, purple};

    public Door (
        Room room1, Room room2, 
        DoorState state, 
        LockedDoorColor color = LockedDoorColor.none) :
        base (room1, room2)
    {
        //Assertions
        if (!Room.IsRoomAdjacent(room1, room2)){
            throw new System.ArgumentException("rooms must be adjacent to each other");
        }

        if (state == DoorState.locked && color == LockedDoorColor.none){
            throw new System.ArgumentException("if the door state is locked, the door color can not be none");
        }

        this.state = state;
        this.color = color;
    }
    
    public void ChangeState(DoorState state){
        this.state = state;
        doorDisplay.ChangeDoorState();
    }

}