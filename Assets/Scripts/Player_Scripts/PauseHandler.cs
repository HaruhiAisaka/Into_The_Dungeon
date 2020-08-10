using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseHandler : MonoBehaviour
{

    public static bool isPaused {get; private set;} = false;

    // Callbacks
    public delegate void Completion();
    public delegate void ToggleCompletion(PauseHandler pauseHandler);

    public void Pause(Completion completion) {
        Pause();
        completion();
    }

    public void Pause() {
        isPaused = true;
        Time.timeScale = 0;
        Debug.Log("PAUSE");
    }

    public void Unpause(Completion completion) {
        Unpause();
        completion();
    }
    public void Unpause() {
        isPaused = false;
        Time.timeScale = 1;
        Debug.Log("UNPAUSE");

    }

    public void TogglePause(ToggleCompletion completion) {
        TogglePause();
        completion(this);
    }

    public void TogglePause() {
        if (isPaused) {
            Unpause();
        } else {
            Pause();
        }
    }
}
