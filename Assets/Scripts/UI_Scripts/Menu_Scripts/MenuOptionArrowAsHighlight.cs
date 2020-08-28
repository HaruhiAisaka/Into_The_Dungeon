using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MenuOptionArrowAsHighlight : MenuOption
{
    [SerializeField] private MenuArrow arrow;
    
    protected override void OnHighlightedUnique(){
        arrow.MoveArrowToOption(this);
    }
}
