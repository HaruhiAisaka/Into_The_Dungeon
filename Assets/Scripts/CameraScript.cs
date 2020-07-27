using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public Transform camera;
    public GameObject player;
    public Vector2 playerLocation;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        playerLocation = player.GetComponent<Transform>().position;
        camera.position = new Vector3(playerLocation.x, playerLocation.y, -1);
    }
}
