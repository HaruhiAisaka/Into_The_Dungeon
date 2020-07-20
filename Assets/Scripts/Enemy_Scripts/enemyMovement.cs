using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyMovement : MonoBehaviour
{
    Rigidbody2D rb;

    float speed = 3f;
    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();       
        InvokeRepeating("move1", 0.01f, 2f);
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
}
