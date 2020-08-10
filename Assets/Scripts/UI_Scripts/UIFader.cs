using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Used to fade UI elements. 
// Must be attached to a UI element with a CanvasGroup component for it to work.
public class UIFader : MonoBehaviour
{
    private CanvasGroup uiElement;
    private Image image;

    private void Start() {
        uiElement = gameObject.GetComponent<CanvasGroup>();
        image = gameObject.GetComponent<Image>();
    }

    public IEnumerator FadeCanvasGroupGradual(float endAlpha, float totalTime){
        float t = 0;
        float initialAlpha = uiElement.alpha;
        while (uiElement.alpha != endAlpha){
            t += Time.deltaTime;
            float newAlpha = Mathf.Lerp(initialAlpha, endAlpha, t / totalTime);
            uiElement.alpha = newAlpha;
            yield return null;
        }
    }

    public IEnumerator FadeCanvasGroupDistinct(float[] alphaValues, float totalTime){
        float t = 0;
        float timeToChangeAlpha = totalTime/alphaValues.Length;
        int i = 0;
        while (i <= alphaValues.Length - 1){
            t += Time.deltaTime;
            if(t >= timeToChangeAlpha){
                uiElement.alpha = alphaValues[i];
                i ++;
                t = 0;
            }
            yield return null;
        }
    }
}
