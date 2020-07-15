using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class enemyScript : MonoBehaviour
{
    Rigidbody2D rb;
    public Transform player;
    float xMin;
    float xMax;

    float speed = 3f;

    float attackCooldown = 0f;

    float meleeRange = 3f;


    private float health = 10f;
    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        xMin = Camera.main.ViewportToWorldPoint( new Vector3(0, 0, 0) ).x;
        xMax= Camera.main.ViewportToWorldPoint( new Vector3(1, 0, 0) ).x;        
        InvokeRepeating("move1", 0.01f, 2f);

    }


    // Update is called once per frame
    void Update()
    {
        //move();

        
    }

    // Enemy moves Towards Player
    void move(){
        Vector2 distanceToPlayer = player.position - transform.position;
        if (distanceToPlayer.magnitude < meleeRange){
            meleeAttack();
        }


        distanceToPlayer.Normalize();
        rb.velocity = distanceToPlayer * speed;
    }

    // Object that generates random numbers
    System.Random rnd = new System.Random();
    // Enemy moves left, right, down, or up randomly
    void move1(){
        int dir = rnd.Next(0,4);
        switch(dir){
            case(0):
                rb.velocity = new Vector2(0,1);
                break;
            case(1):
                rb.velocity = new Vector2(1,0);
                break;
            case(2):
                rb.velocity = new Vector2(0,-1);
                break;
            case(3):
                rb.velocity = new Vector2(-1,0);
                break;

        }
        rb.velocity = rb.velocity * speed;
    }

    void OnTriggerEnter2D(Collider2D collision){
        Debug.Log("collision");
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
        dropLoot();
        Debug.Log("die");
    }
}

