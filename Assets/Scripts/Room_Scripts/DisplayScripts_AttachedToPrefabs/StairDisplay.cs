using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairDisplay : MonoBehaviour
{
    public Stair stair;
    private bool canEnterStairs = true;

    #region Animation Fields
    private float[] alphaValuesFadeBlack = {.3f,.7f,1f};
    private float[] alphaValuesFadeReturn = {.7f,.3f,0f};
    private float timeToFade = .5f;
    #endregion
    
    private UIFader fader;
    private Player player;
    private CameraMovement mainCamera;
    private CurrentRoom currentRoom;

    private void Start() {
        mainCamera = FindObjectOfType<CameraMovement>();
        player = FindObjectOfType<Player>();
        currentRoom = FindObjectOfType<CurrentRoom>();
        fader = GameObject.Find("FaderBlack").GetComponent<UIFader>();
    }   

    IEnumerator StairAnimation(){
        // Freezes player Movement
        player.FreezePlayer();

        // Instantiate the next room
        Room nextRoom = stair.GetNextRoom(currentRoom.GetCurrentRoom());
        RoomGenerator newRoomGenerator = 
            FindObjectOfType<Dungeon>().InstantiateRoom(nextRoom);
        // Initially Disables all Stair Animations
        newRoomGenerator.EnableStairAnimations(false);
        // Moves player to the center of the stairs.
        Coroutine a = StartCoroutine(player.MovePlayerToPoint(
            (Vector2) this.transform.position,
            player.GetSpeed()));
        yield return a;

        // Fades the camera to black
        Coroutine b = StartCoroutine(
            fader.FadeCanvasGroupDistinct(alphaValuesFadeBlack,timeToFade));
        yield return b;

        // Moves the camera and player to the next room.
        player.transform.position = 
            stair.GetStairWorldPosition(nextRoom);
        mainCamera.SetCameraToNewRoom(nextRoom);

        // Updates the current room
        currentRoom.SetCurrentRoom(nextRoom);

        // Defades the camera
        Coroutine c = StartCoroutine(
            fader.FadeCanvasGroupDistinct(alphaValuesFadeReturn,timeToFade));
        yield return c;

        // Enables stair animations
        newRoomGenerator.EnableStairAnimations(true);

        // Destroys current room generator
        Destroy(GetComponentInParent<RoomGenerator>().gameObject);

        // Unfreezes the player
        player.UnfreezePlayer();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (canEnterStairs && other.GetComponent<AltPlayerHitBox>()){
            canEnterStairs = false;
            StartCoroutine(StairAnimation());
        }
    }
    private void OnTriggerExit2D(Collider2D other) {
        if (other.GetComponent<AltPlayerHitBox>()){
            canEnterStairs = true;
        }
    }

    public void EnableAnimations(bool enable){
        canEnterStairs = enable;
    }
}
