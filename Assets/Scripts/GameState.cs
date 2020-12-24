using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    public bool isPaused {get; private set;} = false;

    [SerializeField] PlayerUI playerUI;
    [SerializeField] Player player;
    [SerializeField] Dungeon dungeon; 

    public void Pause() {
        isPaused = true;
        player.FreezePlayer();
        playerUI.EnableMainUI();
    }
    
    public void Unpause() {
        isPaused = false;
        player.UnfreezePlayer();
        playerUI.DisableMainUI();
    }

    public void TogglePause() {
        if (isPaused) {
            Unpause();
        } else {
            Pause();
        }
    }
}
