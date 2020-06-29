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

    float attackCooldown = 0f;

    float meleeRange = 3f;
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
        Vector2 distanceToPlayer = player.position - transform.position;
        if (distanceToPlayer.magnitude < meleeRange){
            meleeAttack();
        }
        
    }

    void moveTowardsPlayer(){

    }


    void meleeAttack(){

    }

    void rangedAttack(){

    }
}

