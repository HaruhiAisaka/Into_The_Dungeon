using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuSelector : MonoBehaviour
{
    [SerializeField] public MenuOption[] options;
    [SerializeField] public int maxOptionsInRow;
    [SerializeField] private MenuOption currentOption;

    // Needed to set the selector position one frame after Start().
    // Nessecary since the currentOption doesn't form until 1 frame after start.

    public MenuOption GetCurrentOption(){
        return currentOption;
    }

    public void SetCurrentOption(MenuOption option){
        if(option == null){
            throw new System.ArgumentNullException("option can not be null");
        }
        else if(Array.Exists(options, element => element == option)){
            currentOption = option;
        }
        else{
            throw new System.ArgumentException("option must be an option stored in options");
        }
    }

    public MenuOption GetUpOption(){
        int currentIndex = Array.IndexOf(options, currentOption);
        int newIndex = currentIndex - maxOptionsInRow;
        if (newIndex < 0) {
            newIndex = options.Length + newIndex;
        } 
        return options[newIndex];
    }

    public MenuOption GetDownOption(){
        int currentIndex = Array.IndexOf(options, currentOption);
        int newIndex = currentIndex + maxOptionsInRow;
        newIndex = newIndex % options.Length;
        return options[newIndex];
    }

    public void LoadCurrentOption(){
        currentOption.OnSelect();
    }

}
