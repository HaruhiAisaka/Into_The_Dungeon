using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuOptionFullScreen : MenuOptionArrowAsHighlight
{
    [SerializeField] Image fill;
    protected override void OnHighlightedUnique(){

    }

    public override void OnSelect(){
        fill.enabled = (!fill.enabled);
    }
}
