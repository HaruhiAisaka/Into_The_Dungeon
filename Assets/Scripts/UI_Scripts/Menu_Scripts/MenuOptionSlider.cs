using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuOptionSlider : MenuOptionArrowAsHighlight, IMenuOptionHorizontalSelect
{
    [SerializeField] Slider slider;

    public override void OnSelect(){

    }

    public void OnHorizontalSelect(){
        float deltaX = Input.GetAxisRaw("Horizontal");
        int sign = Math.Sign(deltaX);
        slider.value += sign;
    }
}
