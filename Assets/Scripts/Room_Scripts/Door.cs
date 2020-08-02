using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    #region Fields Unique to Door
    [SerializeField] private CardinalDirection.Direction4 direction = CardinalDirection.Direction4.ZERO_VECTOR;
    [SerializeField] private DoorState state = DoorState.open;
    [SerializeField] private LockedDoorColor lockedDoorColor = LockedDoorColor.none;
    
    #endregion

    #region Fields shared with all doors.
    [Header("Shared With All Doors")]
    // The amount of time the player must push on the door until it is unlocked.
    private const float unlockDelay = 0.5f;
    private float currentUnlockDelay;
    
    // DoorState is the state of the door. 
    // There are currenlty 4 states though more will be added.
    // None means that there is no door, 
    // the sprite renderer is disabled and what is left is the doorCollider.
    public enum DoorState {none, open, closed, locked};
    public enum LockedDoorColor {none, red, blue, green, yellow, purple};

    private Player player;
    private CurrentRoom currentRoom;
    private CameraMovement mainCamera;
    private Animator playerAnimator;

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
    [SerializeField] private BoxCollider2D doorCollider;

    private Dictionary<LockedDoorColor, Sprite[]> lockedDoorSprites = 
        new Dictionary<LockedDoorColor, Sprite[]>();
    
    #endregion

    void Start(){
        lockedDoorSprites.Add(LockedDoorColor.red, lockedRedDoor);
        lockedDoorSprites.Add(LockedDoorColor.blue, lockedBlueDoor);
        lockedDoorSprites.Add(LockedDoorColor.green, lockedGreenDoor);
        lockedDoorSprites.Add(LockedDoorColor.yellow, lockedYellowDoor);
        lockedDoorSprites.Add(LockedDoorColor.purple, lockedPurpleDoor);
        currentUnlockDelay = unlockDelay;
        currentRoom = FindObjectOfType<CurrentRoom>();
        player = FindObjectOfType<Player>();
        mainCamera = FindObjectOfType<CameraMovement>();
        playerAnimator = player.GetComponent<Animator>();
        ChangeDoorState(state);
        SetDirection();
    }

    // Sets which way the door is facing given the current roomCoordinate.
    private void SetDirection(){
        Vector2 roomCenter = currentRoom.GetCurrentRoomCoordinate().GetRoomWorldPosition();
        Vector2 directionVector = (Vector2) this.transform.position - roomCenter;
        direction = CardinalDirection.Vector2ToCardinalDirection4(directionVector);
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

    private void ChangeDoorState(DoorState state){
        this.state = state;
        switch (state){
            case DoorState.none:
                SpriteRenderersEnable(false);
                doorCollider.enabled = true;
                break;
            case DoorState.open:
                doorCollider.enabled = false;
                SetDoorSprite(openDoor);
                break;
            case DoorState.closed:
                doorCollider.enabled = true;
                SetDoorSprite(closedDoor);
                break;
            case DoorState.locked:
                if (lockedDoorColor == LockedDoorColor.none){
                    throw new System.ArgumentException("If door state is locked, color can not be null", "lockedDoorColor");
                }
                doorCollider.enabled = true;
                SetDoorSprite(lockedDoorSprites[this.lockedDoorColor]);
                break;
        }
            
    }

    #region DoorEnterAnimation
    // Triggers door animation when player enters door.
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.GetComponent<Player>()){
            StartCoroutine(DoorAnimation());
        }
    }

     IEnumerator DoorAnimation(){
        player.FreezePlayer();
        
        if (direction == CardinalDirection.Direction4.NORTH){
            playerAnimator.SetBool("walkLeft", false);
            playerAnimator.SetBool("walkRight", false);
            playerAnimator.SetBool("walkUp", true);
        }
        else if(direction == CardinalDirection.Direction4.SOUTH){
            playerAnimator.SetBool("walkLeft", false);
            playerAnimator.SetBool("walkRight", false);
            playerAnimator.SetBool("walkDown", true);
        }

        //Move player into the door
        Coroutine a = 
            StartCoroutine(player.MovePlayerToPoint(
                (Vector2)this.transform.position,
                player.GetSpeed()));
        yield return a;
        //Update the current room
        Room nextRoom = currentRoom.GetCurrentRoom().GetAdjacentRoom(direction);
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
                CardinalDirection.CardinalDirection4ToVector2(direction, distanceFromDoor),
                player.GetSpeed()));
        yield return c;
        SetDirection();
        player.UnfreezePlayer();
    }

    #endregion

    public void OpenDoor(){
        ChangeDoorState(DoorState.open);
    }

    public void CloseDoor(){
        ChangeDoorState(DoorState.closed);
    }

    public void LockDoor(){
        if (lockedDoorColor == LockedDoorColor.none){
            throw new System.InvalidOperationException("The door does not have a color assigned. Only doors with colors assigned to them can be locked.");
        }
        else{
            ChangeDoorState(DoorState.locked);
        }
    }

    public void UnlockDoor(){
        OpenDoor();
    }

    //Called when the Door Collider has it's CollisionStay2D triggered. 
    //Called whenever any object is touching the door.
    public void CollisionStay(Collision2D other) {
        // Need to check if the player is going towards the door.
        if(other.gameObject.GetComponent<Player>() && this.state == DoorState.locked){
            GameObject player = other.gameObject;
            bool correctKey = player.GetComponent<PlayerInventory>().KeyInInventory(lockedDoorColor);
            float distancePlayerFromDoor = 
                Vector2.Distance(player.transform.position, 
                this.transform.position);
            Vector2 normalizedPlayerVelocity =
                player.GetComponent<Player>().GetPlayerDirection();
            float distancePlayerPlusPlayerVelocityFromDoor = 
                Vector2.Distance((Vector2) player.transform.position + 
                normalizedPlayerVelocity,
                this.transform.position);
            bool playerGoingTowardsDoor = 
                (distancePlayerPlusPlayerVelocityFromDoor < distancePlayerFromDoor);
            if(currentUnlockDelay <= 0 && playerGoingTowardsDoor && correctKey){
                UnlockDoor();
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
        if(other.gameObject.GetComponent<Player>() && this.state == DoorState.locked){
            currentUnlockDelay = unlockDelay;
        }
    }

}
