using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainUI : MonoBehaviour
{
    private void Start() {
        this.gameObject.SetActive(false);
    }
    public void EnableMainUI(){
        this.gameObject.SetActive(true);
    }
    
    public void DisableMainUI(){
        this.gameObject.SetActive(false);
    }
}
