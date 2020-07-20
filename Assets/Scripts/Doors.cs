using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Doors : MonoBehaviour
{   
    [Header("Animation Settings")]
    /*Delay between when the player is first dirrected to enter the door 
    and when the camera starts to move to the over room.*/
    public float cameraMoveDelay = 1f;
    public float cameraMovementTime = 2f;

    /*Used to determine how far the player should walk until he is 
    considered away from the door.*/
    public float distanceFromDoor = 2f;

    private CurrentRoom currentRoom;
    private Player player;
    private Camera mainCamera;
    private Animator playerAnimator;
    private enum Direction{NORTH,SOUTH,EAST,WEST};
    private Direction doorEntered;


    private void Start() {
        currentRoom = FindObjectOfType<CurrentRoom>();
        player = FindObjectOfType<Player>();
        mainCamera = FindObjectOfType<Camera>();
        playerAnimator = player.GetComponent<Animator>();
    }

    /*Even though it's public, do not call this function. 
    I will figure out how to make it private latter while still 
    alowing children to call it.*/
    public void DoorEntered() {
        //Determine what door entered;
        doorEntered = WhichDoorEntered();
        //Perfom Door Animation
        StartCoroutine(DoorAnimation());
    }
    private Direction WhichDoorEntered(){
        int xFactor = currentRoom.GetXFactor();
        int yFactor = currentRoom.GetYFactor();
        //Gets needed information
        Vector2 newRoomCenter = currentRoom.GetRealRoomCoordinate();
        Vector2 playerPosition = player.transform.position;
        float xPlayerDistanceFromCenter = 
            playerPosition.x / xFactor;
        float yPlayerDistanceFromCenter = 
            playerPosition.y / yFactor;
        if (Mathf.Abs(xPlayerDistanceFromCenter) > Mathf.Abs(yPlayerDistanceFromCenter)){
            if (xPlayerDistanceFromCenter < currentRoom.GetRoomCoordinate().x){
                return Direction.WEST;
            } 
            else {
                return Direction.EAST;
            }
        }
        else {
            if (yPlayerDistanceFromCenter < currentRoom.GetRoomCoordinate().y){
                return Direction.SOUTH;
            }
            else {
                return Direction.NORTH;
            }
        }
    }

    IEnumerator DoorAnimation(){
        player.FreezePlayer();
        if (doorEntered == Direction.NORTH){
            playerAnimator.SetBool("walkLeft", false);
            playerAnimator.SetBool("walkRight", false);
            playerAnimator.SetBool("walkUp", true);
        }
        else if(doorEntered == Direction.SOUTH){
            playerAnimator.SetBool("walkLeft", false);
            playerAnimator.SetBool("walkRight", false);
            playerAnimator.SetBool("walkDown", true);
        }
        //Move player into the door
        Coroutine a = StartCoroutine(MovePlayerIntoDoor());
        yield return a;
        //Update the current room
        currentRoom.UpdateRoomByDoor();
        //Moves camera to new room
        Coroutine b = StartCoroutine(MoveCameraToNewRoom());
        yield return b;
        // Move player out of the door into the room
        Coroutine c = StartCoroutine(MovePlayerOutOfDoor());
        yield return c;
        player.UnfreezePlayer();

    }

    IEnumerator MovePlayerIntoDoor(){
        Vector2 roomCenter = currentRoom.GetRealRoomCoordinate();
        Vector2 doorCenter;
        Vector2 playerPosition = player.transform.position;
        int xFactor = currentRoom.GetXFactor();
        int yFactor = currentRoom.GetYFactor();
        switch(doorEntered){
            case Direction.NORTH:
                doorCenter = new Vector2(roomCenter.x, roomCenter.y + yFactor/2);
                break;
            case Direction.SOUTH:
                doorCenter = new Vector2(roomCenter.x, roomCenter.y - yFactor/2);
                break;
            case Direction.EAST:
                doorCenter = new Vector2(roomCenter.x + xFactor/2, roomCenter.y);
                break;
            case Direction.WEST:
                doorCenter = new Vector2(roomCenter.x - xFactor/2, roomCenter.y);
                break;
            default:
                Debug.Log("Should not happen");
                doorCenter = new Vector2(roomCenter.x, roomCenter.y);
                break;
        }
        float t = 0f;
        float distance = Vector2.Distance(playerPosition, doorCenter);
        while (Vector2.Distance(player.transform.position, doorCenter)>0){
            // player.FreezePlayer();
            t += Time.deltaTime;
            player.transform.position = 
                Vector2.Lerp(playerPosition, doorCenter, t/(distance/player.GetSpeed()));
            yield return null;
        }
    }

    IEnumerator MoveCameraToNewRoom(){
        Vector3 cameraPosition = mainCamera.transform.position;
        Vector3 newCenter = 
            new Vector3(currentRoom.GetRealRoomCoordinate().x, 
            currentRoom.GetRealRoomCoordinate().y, cameraPosition.z); 
        float t = 0;
        while (Vector3.Distance(mainCamera.transform.position, newCenter)>0){
            // player.FreezePlayer();
            t += Time.deltaTime;
            mainCamera.transform.position = Vector3.Lerp(cameraPosition, newCenter, t / cameraMovementTime);
            yield return null;
        }
    }

    IEnumerator MovePlayerOutOfDoor(){
        Vector2 pointOutOfDoor;
        Vector2 playerPos = player.transform.position;
        switch(doorEntered){
            case Direction.NORTH:
                pointOutOfDoor = 
                    new Vector2(playerPos.x, playerPos.y + distanceFromDoor);
                break;
            case Direction.SOUTH:
                pointOutOfDoor =
                    new Vector2(playerPos.x, playerPos.y - distanceFromDoor);
                break;
            case Direction.EAST:
                pointOutOfDoor = 
                    new Vector2(playerPos.x + distanceFromDoor, playerPos.y);
                break;
            case Direction.WEST:
                pointOutOfDoor = 
                    new Vector2(playerPos.x - distanceFromDoor, playerPos.y);
                break;
            default:
                Debug.Log("Should not happen");
                pointOutOfDoor = new Vector2(0,0);
                break;
        }
        float distance = Vector2.Distance(player.transform.position,pointOutOfDoor);
        float t = 0;
        while (Vector2.Distance(player.transform.position,pointOutOfDoor)>0){
            // player.FreezePlayer();
            t += Time.deltaTime;
            player.transform.position = Vector2.Lerp(playerPos, pointOutOfDoor, t/(distance/player.GetSpeed()));
            yield return null;
        }
    }
}
