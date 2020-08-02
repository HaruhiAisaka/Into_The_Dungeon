﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : Enemy
{   
    [SerializeField] public GameObject arrowPrefab;
    Transform player;

    private float shootSpeed = 3f;

    void Start(){
        player =  GameObject.Find("Player").transform;
    }
    //Starts moving to the right
    public override void DerivedStart()
    {
        rb.velocity = Vector2.zero; 
        InvokeRepeating("Shoot", 1f, 4f); 
    }

    public override void DerivedUpdate(){}

    public override void Move(){
    }

    // Bounces off walls
    void OnTriggerEnter2D(Collider2D collision){
        if (collision.gameObject.tag == "wall"){
            rb.velocity *= -1;
        }
    }

    void Shoot(){
        Debug.Log("shoot");
        GameObject laser = Instantiate(arrowPrefab, transform.position, Quaternion.identity);


        Vector2 distanceToPlayer = player.position - transform.position;
        distanceToPlayer.Normalize();
        laser.GetComponent<Rigidbody2D>().velocity = distanceToPlayer * shootSpeed;



    }
}
