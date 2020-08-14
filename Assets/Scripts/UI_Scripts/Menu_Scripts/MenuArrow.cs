using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuArrow : MonoBehaviour
{
    [SerializeField] private MenuSelector menuSelector;
    [SerializeField] private float yOffSet;
    [SerializeField] private bool freezeArrow;
    private float dasDelay = .4f;
    private float regularDelay = .05f;

    private Coroutine horizontalSelect;
    private float deltaX;

    private void Start() {
        MoveArrowToOption(menuSelector.GetCurrentOption());
    }

    private void Update() {
        // Updates Selected Option when the player pushes one of the vertical Options.
        if(Input.GetButtonDown("Vertical")&&!freezeArrow){
            MoveArrow();
            if (horizontalSelect != null) StopCoroutine(horizontalSelect);
        }
        else if(Input.GetKeyDown(KeyCode.Space)||Input.GetKeyDown(KeyCode.Return)){
            menuSelector.LoadCurrentOption();
        }
        else if(Input.GetButtonDown("Horizontal")){
            MenuOption currentOption = menuSelector.GetCurrentOption();
            if (currentOption is IMenuOptionHorizontalSelect currentOptionH){
                if (horizontalSelect != null) StopCoroutine(horizontalSelect);
                horizontalSelect = StartCoroutine(HorizontalSelectDAS(currentOptionH));
            }
        }

        Debug.Log(horizontalSelect);

    }

    private void MoveArrow(){
        float deltaY  = Input.GetAxisRaw("Vertical");
        MenuOption nextOption;
        if (deltaY == 0){
            return;
        }
        else if(deltaY < 0){
            nextOption = menuSelector.GetDownOption();
        }
        else{
            nextOption = menuSelector.GetUpOption();
        }
        menuSelector.SetCurrentOption(nextOption);
        MoveArrowToOption(nextOption);
    }

    public void MoveArrowToOption(MenuOption option){
        if(horizontalSelect != null) StopCoroutine(horizontalSelect);
        RectTransform arrow = this.GetComponent<RectTransform>();
        RectTransform optionRect = option.GetComponent<RectTransform>();
        Vector2 anchorPoint = optionRect.anchoredPosition;
        float newYPos = optionRect.anchoredPosition.y + yOffSet;
        arrow.anchoredPosition = new Vector2(arrow.anchoredPosition.x, newYPos);
    }

    public void FreezeArrow(bool freeze){
        freezeArrow = freeze;
    }

    private IEnumerator HorizontalSelectDAS(IMenuOptionHorizontalSelect option){
        option.OnHorizontalSelect();
        yield return new WaitForSeconds(dasDelay);
        while (true){
            option.OnHorizontalSelect();
            yield return new WaitForSeconds(regularDelay);
        }
    }
}
