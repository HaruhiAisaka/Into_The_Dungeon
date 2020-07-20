using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyMovement1 : MonoBehaviour
{
    public Transform player;
    Rigidbody2D rb;
    float speed = 3f;


    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();           
    }

    // Update is called once per frame
    void Update()
    {
        move();
    }

    //enemy moves towards player
    void move(){
        Vector2 distanceToPlayer = player.position - transform.position;
        distanceToPlayer.Normalize();
        rb.velocity = distanceToPlayer * speed;
    }
}
