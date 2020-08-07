using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blob : Enemy
{   
    //Starts moving to the right
    public override void DerivedStart()
    {
        rb.velocity = new Vector2(0 ,speed);    
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
}
