using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State {
    InGame,
    InMenu,
    Paused
}

public static class GameState {

    private static State gameState = State.InGame;

    // Pause Callbacks
    public delegate void Completion();

    public static bool isPaused() {
        return gameState == State.Paused;
    }

    public static void EnterMenu() {
        // TODO
        //UnityEngine.SceneManangement.SceneManager.LoadScene("?");
        //gameState = State.InMenu;
    }

    public static void ExitMenu() {
        // TODO
        //UnityEngine.SceneManangement.SceneManangement.LoadScene("?");
        //gameState = State.InGame;
    }

    public static void Pause(Completion completion) {
        Pause();
        completion();
    }

    public static void Pause() {
        gameState = State.Paused;
        Time.timeScale = 0;
        Debug.Log("PAUSE");
    }

    public static void Unpause(Completion completion) {
        Unpause();
        completion();
    }
    public static void Unpause() {
        gameState = State.InGame;
        Time.timeScale = 1;
        Debug.Log("UNPAUSE");

    }

    public static void TogglePause(Completion completion) {
        TogglePause();
        completion();
    }

    public static void TogglePause() {
        if (gameState == State.Paused) {
            Unpause();
        } else {
            Pause();
        }
    }
}
