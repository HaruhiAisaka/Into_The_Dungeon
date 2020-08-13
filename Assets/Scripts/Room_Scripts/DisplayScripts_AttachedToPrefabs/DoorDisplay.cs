using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorDisplay : MonoBehaviour
{
    public Door door = null;

    public Cardinal4.Direction direction;

    #region Fields shared with all doors.
    [Header("Shared With All DoorsDisplays")]
    // The amount of time the player must push on the door until it is unlocked.
    private const float unlockDelay = 0.3f;
    private float currentUnlockDelay;

    private Player player;
    private CurrentRoom currentRoom;
    private CameraMovement mainCamera;
    private Animator playerAnimator;

    [SerializeField] private RoomGenerator currentRoomGenerator;

    [Header("Animation Settings")]
    [SerializeField] private float cameraMovementTime = 2f;

    /*Used to determine how far the player should walk until he is 
    considered away from the door.*/
    [SerializeField] private float distanceFromDoor = 2f;

    [Header("Door Sprites")]
    [SerializeField] private Sprite[] openDoor;
    [SerializeField] private Sprite[] closedDoor;
    [SerializeField] private Sprite[] lockedRedDoor;
    [SerializeField] private Sprite[] lockedBlueDoor;
    [SerializeField] private Sprite[] lockedGreenDoor;
    [SerializeField] private Sprite[] lockedYellowDoor;
    [SerializeField] private Sprite[] lockedPurpleDoor;

    [Header("DoorParts")]
    [SerializeField] private SpriteRenderer[] doorBottom;
    [SerializeField] private SpriteRenderer[] doorTop;
    [SerializeField] private BoxCollider2D doorClosedCollider;
    [SerializeField] private BoxCollider2D doorOpenHitBox;

    private Dictionary<Door.LockedDoorColor, Sprite[]> lockedDoorSprites = 
        new Dictionary<Door.LockedDoorColor, Sprite[]>();
    
    #endregion

    void Start(){
        lockedDoorSprites.Add(Door.LockedDoorColor.red, lockedRedDoor);
        lockedDoorSprites.Add(Door.LockedDoorColor.blue, lockedBlueDoor);
        lockedDoorSprites.Add(Door.LockedDoorColor.green, lockedGreenDoor);
        lockedDoorSprites.Add(Door.LockedDoorColor.yellow, lockedYellowDoor);
        lockedDoorSprites.Add(Door.LockedDoorColor.purple, lockedPurpleDoor);
        currentUnlockDelay = unlockDelay;
        currentRoom = FindObjectOfType<CurrentRoom>();
        player = FindObjectOfType<Player>();
        mainCamera = FindObjectOfType<CameraMovement>();
        playerAnimator = player.GetComponent<Animator>();
        ChangeDoorState();
    }

    // Sets the sprites for the door. 
    // The top part of the door sprite should be the first element of sprites. 
    // The bottom part of the door sprite should be the second element.
    private void SetDoorSprite(Sprite[] sprites){
        SpriteRenderersEnable(true);
        Sprite doorTop = sprites[0];
        Sprite doorBottom = sprites[1];
        this.doorTop[0].sprite = doorTop;
        this.doorTop[1].sprite = doorTop;
        this.doorBottom[0].sprite = doorBottom;
        this.doorBottom[1].sprite = doorBottom;
    }

    // Enables or disables all sprite renderers of this door.
    private void SpriteRenderersEnable(bool enable){
        this.doorTop[0].enabled = enable;
        this.doorTop[1].enabled = enable;
        this.doorBottom[0].enabled = enable;
        this.doorBottom[1].enabled = enable;
    }

    // Changes the door state of this display according to the stored door object.
    public void ChangeDoorState(){
        if (door == null){
            SpriteRenderersEnable(false);
            doorClosedCollider.enabled = true;
            return;
        }
        Door.DoorState state = door.state;
        switch (state){
            case Door.DoorState.open:
                doorClosedCollider.enabled = false;
                SetDoorSprite(openDoor);
                break;
            case Door.DoorState.closed:
                doorClosedCollider.enabled = true;
                SetDoorSprite(closedDoor);
                break;
            case Door.DoorState.locked:
                if (door.color == Door.LockedDoorColor.none){
                    throw new System.ArgumentException("If door state is locked, color can not be null", "lockedDoorColor");
                }
                doorClosedCollider.enabled = true;
                SetDoorSprite(lockedDoorSprites[door.color]);
                break;
        }
            
    }

    #region DoorEnterAnimation
    // Enable Door Animations, enables the ability for the door to do animations
    public void EnableAnimations(bool enable){
        doorOpenHitBox.enabled = enable;
    }

    // Triggers door animation when player enters door.
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.GetComponent<Player>()){
            StartCoroutine(DoorAnimation());
        }
    }

    IEnumerator DoorAnimation(){
        // Freezes player movement
        player.FreezePlayer();
        // Instantiates the NextRoom
        RoomGenerator newRoomGenerator = 
            FindObjectOfType<DungeonDisplay>()
            .InstantiateRoom(door.GetNextRoom(currentRoom.GetCurrentRoom()));

        // Disables Door Animations For the new Room
        newRoomGenerator.EnableDoorAnimations(false);

        // Changes the player animation to walk in appropreate direction.
        if (direction == Cardinal4.Direction.NORTH){
            playerAnimator.SetFloat("deltaX", 0);
            playerAnimator.SetFloat("deltaY", 1);
        }
        else if(direction == Cardinal4.Direction.SOUTH){
            playerAnimator.SetFloat("deltaX", 0);
            playerAnimator.SetFloat("deltaY", -1);
        }

        //Move player into the door
        Coroutine a = 
            StartCoroutine(player.MovePlayerToPoint(
                (Vector2)this.transform.position,
                player.GetSpeed()));
        yield return a;
        //Update the current room
        Room nextRoom = door.GetNextRoom(currentRoom.GetCurrentRoom());
        currentRoom.SetCurrentRoom(nextRoom);
        //Moves camera to new room
        Coroutine b = 
            StartCoroutine(mainCamera.MoveCameraToNewRoom(
            currentRoom.GetCurrentRoom(),
            cameraMovementTime));
        yield return b;
        // Move player out of the door into the room
        Coroutine c = 
            StartCoroutine(player.MovePlayerToPoint(
                (Vector2)this.transform.position + 
                Cardinal4.DirectionToVector2(direction, distanceFromDoor),
                player.GetSpeed()));
        yield return c;

        // Enable Door Animations for the Next Room
        newRoomGenerator.EnableDoorAnimations(true);
        // Allow for Player Movement
        player.UnfreezePlayer();
        // Destroy current RoomGenerator
        Destroy(currentRoomGenerator.gameObject);
    }

    #endregion


    //Called when the Door Collider has it's CollisionStay2D triggered. 
    //Called whenever any object is touching the door.
    public void CollisionStay(Collision2D other) {
        if(door == null) return;
        // Need to check if the player is going towards the door.
        if(other.gameObject.GetComponent<Player>() && door.state == Door.DoorState.locked){
            GameObject player = other.gameObject;
            bool correctKey = 
                player.GetComponent<PlayerInventory>().
                KeyInInventory(door.color);
            Cardinal4.Direction playerDirection = 
                Cardinal4.Vector2ToDirection(player.GetComponent<Player>().GetPlayerDirectionVector());
            bool playerGoingTowardsDoor = (playerDirection == direction);
            if(currentUnlockDelay <= 0 && playerGoingTowardsDoor && correctKey){
                door.ChangeState(Door.DoorState.open);
            }
            else if (playerGoingTowardsDoor && correctKey){
                currentUnlockDelay -= 1 * Time.deltaTime;
            }
            else{
                currentUnlockDelay = unlockDelay;
            }
        }
    }

    public void CollisionExit(Collision2D other) {
        if (door == null) return;
        if(other.gameObject.GetComponent<Player>() 
            && door.state == Door.DoorState.locked){
            currentUnlockDelay = unlockDelay;
        }
    }

}
