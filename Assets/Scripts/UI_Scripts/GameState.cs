using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scenes {
    private Scenes(string value) { Value = value; }

    public string Value { get; set; }

    public static Scenes Game { get { return new Scenes("LiamR_Dungeon_Room"); } }
    public static Scenes Menu { get { return new Scenes("MapHUD"); } }
}

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
        gameState = State.InMenu;
        SceneManager.LoadScene(Scenes.Menu.Value);
    }

    public static void ExitMenu() {
        gameState = State.InGame;
        SceneManager.LoadScene(Scenes.Game.Value);
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
