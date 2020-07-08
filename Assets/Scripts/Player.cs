﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 1f;
    public Transform player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // MoveHorizontal();
        // MoveVertical();
        Move();
    }

    private void Move() {
        if (Input.GetKey("left") || Input.GetKey("a")) {
            float deltaX = -speed * Time.deltaTime;
            player.position = new Vector2( player.position.x + deltaX, player.position.y );
        }
        if (Input.GetKey("right") || Input.GetKey("d")) {
            float deltaX = speed * Time.deltaTime;
            player.position = new Vector2( player.position.x + deltaX, player.position.y );
        }
        if (Input.GetKey("up") || Input.GetKey("w")) {
            float deltaY = speed * Time.deltaTime;
            player.position = new Vector2( player.position.x, player.position.y + deltaY );
        }
        if (Input.GetKey("down") || Input.GetKey("s")) {
            float deltaY = -speed * Time.deltaTime;
            player.position = new Vector2( player.position.x, player.position.y + deltaY );
        }
    }
    
    // Move horizontally: left if `a` or left-button is pressed, right if `d` or right-button is pressed
    private void MoveHorizontal() {
        float deltaX = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        player.position = new Vector2( player.position.x + deltaX, player.position.y );
    }

    // Move vertically
    private void MoveVertical() {
        float deltaY = Input.GetAxis("Vertical") * speed * Time.deltaTime;
        player.position = new Vector2( player.position.x, player.position.y + deltaY );
    }
}
