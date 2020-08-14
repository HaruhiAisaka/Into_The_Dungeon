using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    // Update is called once per frame
    void Update() {
        if (Input.GetButtonDown("Pause")) {
            GameState.ExitMenu();
        }
    }
}
