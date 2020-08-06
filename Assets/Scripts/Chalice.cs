using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Chalice : MonoBehaviour
{   
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.GetComponent<Player>()){
            Player player = FindObjectOfType<Player>();
            player.FreezePlayer();
        }
    }
}
