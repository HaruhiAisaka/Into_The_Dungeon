using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : Enemy
{
    //Starts moving to the right
    public override void DerivedStart() {
        rb.velocity = new Vector2(0,speed);
    }

    public override void DerivedUpdate() { }
    System.Random rnd = new System.Random();
    private float elapsed = 0f;

    public override void Move()
    {
        if (elapsed > 2f)
        {
            int dir = rnd.Next(0, 4);
            switch (dir)
            {
                case (0):
                    rb.velocity = new Vector2(0, 1);
                    break;
                case (1):
                    rb.velocity = new Vector2(1, 0);
                    break;
                case (2):
                    rb.velocity = new Vector2(0, -1);
                    break;
                case (3):
                    rb.velocity = new Vector2(-1, 0);
                    break;

            }
            rb.velocity = rb.velocity * speed; 
            elapsed = 0f;
        }
        elapsed += Time.deltaTime;

    }

    // Bounces off walls
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "wall" || collision.gameObject.tag == "door")
        {
            rb.velocity *= -1;
        }


    }
}


