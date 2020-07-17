using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyMovement3 : MonoBehaviour
{
    Rigidbody2D rb;

    float speed = 3f;
    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();   
        rb.velocity = new Vector2(0, speed);    
    }


}
