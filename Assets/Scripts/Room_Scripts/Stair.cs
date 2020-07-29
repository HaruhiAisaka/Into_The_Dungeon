using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stair : MonoBehaviour
{
    [SerializeField] private Stair nextStair;

    [SerializeField] public bool canEnterStairs = true;
    
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.GetComponent<Player>() && canEnterStairs){
            Player player = other.GetComponent<Player>();
            nextStair.canEnterStairs = false;
            player.transform.position = nextStair.transform.position;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.GetComponent<Player>()){
            canEnterStairs = true;
        }
    }
}
