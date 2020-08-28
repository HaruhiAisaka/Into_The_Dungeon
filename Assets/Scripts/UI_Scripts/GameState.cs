using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scenes {
    private Scenes(string value) { Value = value; }

    public string Value { get; set; }

    public static Scenes Game { get { return new Scenes("LiamR_Dungeon_Room"); } }
}

public enum State {
    InGame,
    InMenu
}

public static class GameState {

    private static State gameState = State.InGame;

    // Pause Callbacks
    public delegate void Completion();

    public static void ToggleMenu(Completion completion) {
        ToggleMenu();
        completion();
    }

    public static void ToggleMenu() {
        if (gameState == State.InMenu) {
            ExitMenu();
        } else {
            EnterMenu();
        }
    }

    public static void EnterMenu() {
        gameState = State.InMenu;
        Time.timeScale = 0;
    }

    public static void ExitMenu() {
        gameState = State.InGame;
        Time.timeScale = 1;
    }

    public static bool InMenu() {
        return gameState == State.InMenu;
    }
}
