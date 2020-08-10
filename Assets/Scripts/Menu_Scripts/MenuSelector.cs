using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuSelector : MonoBehaviour
{
    [SerializeField] RectTransform selector;
    [SerializeField] Button[] buttons;

    private void Update() {
        UpdateSelectorPosition();
        Vector3[] corners = new Vector3[4];
        buttons[0].GetComponent<RectTransform>().GetLocalCorners(corners);
        Debug.Log(corners[0]);
        Debug.Log(corners[1]);
        Debug.Log(corners[2]);
        Debug.Log(corners[3]);
    }

    private void UpdateSelectorPosition(){

    }

    private void MoveSelectorToButton(Button button){
        RectTransform buttonRect = button.GetComponent<RectTransform>();
    }
}
