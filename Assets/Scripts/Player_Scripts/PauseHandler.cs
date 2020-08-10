using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseHandler : MonoBehaviour
{

    public bool isPaused {get; private set;} = false;

    public void Pause() {
        isPaused = true;
        Time.timeScale = 0;
    }

    public void Unpause() {
        isPaused = false;
        Time.timeScale = 1;

    }

    public void TogglePause() {
        if (isPaused) {
            Unpause();
        } else {
            Pause();
        }
    }
}
