using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Curtains : MonoBehaviour
{
    [SerializeField] private GameObject leftCurtain;
    [SerializeField] private GameObject rightCurtain;

    private Camera mainCamera;

    private float screenLength;

    private void Start() {
        mainCamera = FindObjectOfType<Camera>();
        screenLength = mainCamera.orthographicSize * mainCamera.aspect;
    }

    // Moves the curtains a certain displacement. 
    // Positive means appart. Negative means closer together.
    private void MoveCurtains(float displacement){
        leftCurtain.transform.position += new Vector3(-displacement,0,0);
        rightCurtain.transform.position += new Vector3(displacement,0,0);
    }

    public IEnumerator OpenCurtains(){
        float totalTime = 1.5f;
        float timeToChangePos = totalTime / (screenLength/.5f);
        float t = 0;
        while (rightCurtain.transform.position.x < screenLength * 1.5){
            t += Time.deltaTime;
            if(t >= timeToChangePos){
                MoveCurtains(.5f);
                t = 0;
            }
            yield return null;
        }
    }

    public void SetCurtainsClosed(){
        leftCurtain.transform.localPosition = 
            new Vector3(Room.LENGTH_FROM_CENTER_X * -.5f - .5f,0,0);
        rightCurtain.transform.localPosition = 
            new Vector3(Room.LENGTH_FROM_CENTER_X * .5f + .5f,0,0);
    }

    public IEnumerator CloseCurtains(){
        float totalTime = 1.5f;
        float timeToChangePos = totalTime / (screenLength/.5f);
        float t = 0;
        while (rightCurtain.transform.position.x > screenLength * .5){
            t += Time.deltaTime;
            if(t >= timeToChangePos){
                MoveCurtains(-.5f);
                t = 0;
            }
            yield return null;
        }
    }
}
