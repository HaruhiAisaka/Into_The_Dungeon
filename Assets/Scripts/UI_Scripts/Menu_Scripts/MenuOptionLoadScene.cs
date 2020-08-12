using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuOptionLoadScene : MenuOptionArrowAsHighlight
{
    [SerializeField] private SceneLoader.SceneName sceneName;
    [SerializeField] private SceneLoader sceneLoader;

    public override void OnSelect(){
        sceneLoader.LoadScene(sceneName);
    }
}
