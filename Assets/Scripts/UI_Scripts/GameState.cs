using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum State {
    InGame,
    InMenu,
    Paused
} 
public class GameState : MonoBehaviour
{

    public PauseHandler pauseHandler;

}
