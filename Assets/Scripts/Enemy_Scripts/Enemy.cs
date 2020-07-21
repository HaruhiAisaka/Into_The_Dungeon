﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Enemy : MonoBehaviour
{
    Rigidbody2D rb;
    float xMin;
    float xMax;

    private bool freezeEnemy = false;

    float speed = 3f;

    float attackCooldown = 0f;

    float meleeRange = 3f;

    public Component movement;


    private float health = 10f;
    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();       

        xMin = Camera.main.ViewportToWorldPoint( new Vector3(0, 0, 0) ).x;
        xMax= Camera.main.ViewportToWorldPoint( new Vector3(1, 0, 0) ).x;        

    }


    // Update is called once per frame
    void Update()
    {
        

        
    }

    

    

    void OnTriggerEnter2D(Collider2D collision){
        if (collision.gameObject.tag == "wall"){
            rb.velocity *= -1;
        }
    

    }
    // Melee attack player
    void meleeAttack(){
        
        // Debug.Log("melee");

    }
    // Range attack player
    void rangedAttack(){

    }
    // Returns the health of this enemy
    public float getHealth(){
        return health;
    }
    /** Reduces health
    * param damage: amount of health lost */
    
    public void takeDamage(float damage){
        health -= damage;
        if (health < 0f){
            die();
        }
    }
    
    // Drop loot on the ground for player to pick up
    private void dropLoot(){

    }

    // Enemy is destroyed and drops loot
    void die(){
        Destroy(gameObject);

        dropLoot();
        
    }

    public void FreezeEnemy(){
        freezeEnemy = true;
    }

    public void UnfreezeEnemy(){
        freezeEnemy = false;
    }

    public bool GetFreezeEnemy(){
        return freezeEnemy;
    }
}
