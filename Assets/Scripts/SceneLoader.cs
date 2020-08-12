using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public enum SceneName {
        title_screen = 0,
        tutorial_dungeon = 1
    }

    private int currentSceneIndex;

    public void RestartScene(){
        Time.timeScale = 1;
        SceneManager.LoadScene(currentSceneIndex);
    }

    public void LoadScene(SceneName sceneName){
        SceneManager.LoadScene((int) sceneName);
    }

    public void LoadTutorial(){
        LoadScene(SceneName.tutorial_dungeon);
    }

    public void QuitGame(){
        Application.Quit();
    }
}
