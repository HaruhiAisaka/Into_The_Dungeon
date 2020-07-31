using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stair : MonoBehaviour
{
    [SerializeField] private Stair nextStair;
    [SerializeField] private bool canEnterStairs = true;

    // Temporary Serialization for testing. Eventually, the variable nextRoom will be initialized when the stair is created based on roomData.
    [SerializeField] private int x;
    [SerializeField] private int y;

    #region Animation Fields
    private float[] alphaValuesFadeBlack = {.3f,.7f,1f};
    private float[] alphaValuesFadeReturn = {.7f,.3f,0f};
    private float timeToFade = .5f;
    #endregion
    
    private UIFader fader;
    private Room nextRoom;
    private Player player;
    private CameraMovement mainCamera;

    private void Start() {
        fader = FindObjectOfType<PlayerUI>().GetComponentInChildren<UIFader>();
        mainCamera = FindObjectOfType<CameraMovement>();
        player = FindObjectOfType<Player>();
        // Temporary
        nextRoom = FindObjectOfType<Dungeon>().GetRoom(new RoomCoordinate(x,y));
    }   

    IEnumerator StairAnimation(){
        player.FreezePlayer();
        Coroutine a = StartCoroutine(player.MovePlayerToPoint(
            (Vector2) this.transform.position,
            player.GetSpeed()));
        yield return a;
        Coroutine b = StartCoroutine(
            fader.FadeCanvasGroupDistinct(alphaValuesFadeBlack,timeToFade));
        yield return b;
        player.transform.position = nextStair.transform.position;
        mainCamera.SetCameraToNewRoom(nextRoom);
        Coroutine c = StartCoroutine(
            fader.FadeCanvasGroupDistinct(alphaValuesFadeReturn,timeToFade));
        yield return c;
        player.UnfreezePlayer();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (canEnterStairs&&other.GetComponent<AltPlayerHitBox>()){
            canEnterStairs = false;
            nextStair.canEnterStairs = false;
            StartCoroutine(StairAnimation());
        }
    }
    private void OnTriggerExit2D(Collider2D other) {
        if (other.GetComponent<AltPlayerHitBox>()){
            canEnterStairs = true;
        }
    }
}
