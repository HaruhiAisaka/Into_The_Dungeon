using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MenuOption : MonoBehaviour
{
    [SerializeField] protected MenuSelector menuSelector;

    // Procedure that is executed when OnHighlighted is called on Any Option.
    public void OnHighlighted(){
        menuSelector.SetCurrentOption(this);
        OnHighlightedUnique();
    }

    // Procedures that are unique to the Menu Option that inherets this class.
    protected abstract void OnHighlightedUnique();
    public abstract void  OnSelect();
}
