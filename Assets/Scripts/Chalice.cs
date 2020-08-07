using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chalice : MonoBehaviour
{   
    private UIFader whiteFader;
    private Animator myAnimator;
    private Player player;

    [Header("Animation Settings")]

    [SerializeField] private Curtains curtains;
    [SerializeField] private float[] whiteShineAlpha;
    [SerializeField] private float shineTime;
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.GetComponent<Player>()){
            StartCoroutine(ChaliceAnimation());
        }
    }


    private IEnumerator ChaliceAnimation(){
        CurrentRoom currentRoom = FindObjectOfType<CurrentRoom>();
        Vector2 currentRoomPos = 
            currentRoom.GetCurrentRoomCoordinate().GetRoomWorldPosition();
        GameObject curtainsGameObject = 
            Instantiate(curtains.gameObject,
            currentRoomPos,
            Quaternion.identity);
        player = FindObjectOfType<Player>();
        player.FreezePlayer();
        player.transform.position = 
            FindObjectOfType<CurrentRoom>().
            GetCurrentRoomCoordinate().
            GetRoomWorldPosition();
        player.GetComponent<SpriteRenderer>().sortingLayerID = SortingLayer.NameToID("AboveAll");
        Animator playerAnimator = player.GetComponent<Animator>();
        playerAnimator.SetTrigger("chalice");
        // Moves Chalice above player
        this.transform.position = 
            (Vector2) player.transform.position + new Vector2(0,1f);
        this.GetComponent<SpriteRenderer>().sortingLayerID = SortingLayer.NameToID("AboveAll");
        whiteFader = GameObject.Find("FaderWhite").GetComponent<UIFader>();
        yield return new WaitForSeconds(.5f);
        Coroutine a = StartCoroutine(whiteFader.FadeCanvasGroupDistinct(whiteShineAlpha,shineTime));
        yield return a;
        yield return new WaitForSeconds(2f);
        Coroutine b = StartCoroutine(curtainsGameObject.GetComponent<Curtains>().CloseCurtains());
    }
}
