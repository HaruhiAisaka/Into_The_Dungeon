using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private DoorState state;
    [SerializeField] private LockedDoorColor lockedDoorColor;
    
    [Header("Shared With All Doors")]
    // The amount of time the player must push on the door until it is unlocked.
    private const float unlockDelay = 0.5f;
    private float currentUnlockDelay;

    public enum DoorState {open, closed, locked};
    public enum LockedDoorColor {none, red, blue, green, yellow, purple};
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
    void Start(){
        lockedDoorSprites.Add(LockedDoorColor.red, lockedRedDoor);
        lockedDoorSprites.Add(LockedDoorColor.blue, lockedBlueDoor);
        lockedDoorSprites.Add(LockedDoorColor.green, lockedGreenDoor);
        lockedDoorSprites.Add(LockedDoorColor.yellow, lockedYellowDoor);
        lockedDoorSprites.Add(LockedDoorColor.purple, lockedPurpleDoor);
        ChangeDoorState(state);
        currentUnlockDelay = unlockDelay;
    }


    // Sets the sprites for the door. 
    // The top part of the door sprite should be the first element of sprites. 
    // The bottom part of the door sprite should be the second element.
    private void SetDoorSprite(Sprite[] sprites){
        Sprite doorTop = sprites[0];
        Sprite doorBottom = sprites[1];
        this.doorTop[0].sprite = doorTop;
        this.doorTop[1].sprite = doorTop;
        this.doorBottom[0].sprite = doorBottom;
        this.doorBottom[1].sprite = doorBottom;
    }

    private void ChangeDoorState(DoorState state){
        this.state = state;
        switch (state){
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

    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.GetComponent<Player>()){
            this.GetComponentInParent<Doors>().DoorEntered();
        }
    }

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
