using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuOptionChangeCanvas : MenuOptionArrowAsHighlight
{
    [SerializeField] private RectTransform currentCanvas;
    [SerializeField] private RectTransform nextCanvas;
    public override void OnSelect(){
        currentCanvas.gameObject.SetActive(false);
        nextCanvas.gameObject.SetActive(true);
    }
}
