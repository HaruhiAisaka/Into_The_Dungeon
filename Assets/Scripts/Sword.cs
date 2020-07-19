using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // When it hits an enemy
    void OnTriggerEnter2D(Collider2D Collision) {
        if (Collision.gameObject.tag == "enemy") {
            Debug.Log("HIT");
            enemyScript es = Collision.gameObject.GetComponent<enemyScript>();
            es.takeDamage(5f);
            // TO IMPLEMENT: damage the enemy
            // TO IMPLEMENT: knock the enemy back
        }
    }
}
