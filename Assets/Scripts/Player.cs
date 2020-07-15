using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Player Movement")]
    public float speed = 1f;

    [SerializeField] Rigidbody2D myRigidBody2D;

    // Start is called before the first frame update
    void Start()
    {
        myRigidBody2D = this.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // MoveHorizontal();
        // MoveVertical();
        Move();
    }

    //Code is not used
    // -------------------------------------------------------------------------
    // private void Move() {
    //     if (Input.GetKey("left") || Input.GetKey("a")) {
    //         float deltaX = -speed * Time.deltaTime;
    //         player.position = new Vector2( player.position.x + deltaX, player.position.y );
    //     }
    //     if (Input.GetKey("right") || Input.GetKey("d")) {
    //         float deltaX = speed * Time.deltaTime;
    //         player.position = new Vector2(player.position.x + deltaX, player.position.y );
    //     }
    //     if (Input.GetKey("up") || Input.GetKey("w")) {
    //         float deltaY = speed * Time.deltaTime;
    //         player.position = new Vector2( player.position.x, player.position.y + deltaY );
    //     }
    //     if (Input.GetKey("down") || Input.GetKey("s")) {
    //         float deltaY = -speed * Time.deltaTime;
    //         player.position = new Vector2( player.position.x, player.position.y + deltaY );
    //     }
    // }
    //-------------------------------------------------------------------------
    
    // Move horizontally: left if `a` or left-button is pressed, right if `d` or right-button is pressed
    private void Move() {
        //Changed Input.GetAxis to Input.GetAxisRaw this gives out values of either 0 or 1.
        float deltaX = Input.GetAxisRaw("Horizontal") * speed;
        float deltaY = Input.GetAxisRaw("Vertical") * speed;
        myRigidBody2D.velocity = new Vector2(deltaX, deltaY);
    }
}
