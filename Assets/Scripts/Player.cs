using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Player Movement")]
    public float speed = 1f;

    public int playerHealth;

    public BoxCollider2D attackHitBox;

    private Rigidbody2D myRigidBody2D;

    private Animator myAnimator;

    // Start is called before the first frame update
    void Start()
    {
        attackHitBox.enabled = false;
        myRigidBody2D = this.GetComponent<Rigidbody2D>();
        myAnimator = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        if(Input.GetButtonDown("Attack1")){
            Attack();
        }
    }
    
    // Move horizontally: left if `a` or left-button is pressed, right if `d` or right-button is pressed
    private void Move() {
        //Changed Input.GetAxis to Input.GetAxisRaw this gives out values of either 0 or 1.
        float deltaX = 0;
        float deltaY = 0;
        deltaX = Input.GetAxisRaw("Horizontal") * speed;
        deltaY = Input.GetAxisRaw("Vertical") * speed;
        myRigidBody2D.velocity = new Vector2(deltaX, deltaY);
        //set animation direction
        if (deltaX > 0) {
            myAnimator.SetBool("walkRight", true);
            myAnimator.SetBool("walkLeft", false);
            myAnimator.SetBool("walkUp", false);
            myAnimator.SetBool("walkDown", false);
        }
        else if (deltaX < 0) {
            myAnimator.SetBool("walkLeft", true);
            myAnimator.SetBool("walkRight", false);
            myAnimator.SetBool("walkUp", false);
            myAnimator.SetBool("walkDown", false);
        }
        else if (deltaY > 0) {
            myAnimator.SetBool("walkUp", true);
            myAnimator.SetBool("walkLeft", false);
            myAnimator.SetBool("walkRight", false);
            myAnimator.SetBool("walkDown", false);
        }
        else if (deltaY < 0) {
            myAnimator.SetBool("walkDown", true);
            myAnimator.SetBool("walkLeft", false);
            myAnimator.SetBool("walkUp", false);
            myAnimator.SetBool("walkRight", false);
        }
    }

    private void Attack(){
        attackHitBox.transform.position = 
            new Vector3(attackHitBox.transform.position.x, 
            attackHitBox.transform.position.y - 1, 
            attackHitBox.transform.position.z);
        myAnimator.SetTrigger("attack");
        attackHitBox.enabled = true;
    }

    private void AttackEnd(){
        attackHitBox.transform.position = 
            new Vector3(attackHitBox.transform.position.x, 
            attackHitBox.transform.position.y + 1, 
            attackHitBox.transform.position.z);
        attackHitBox.enabled = false;
    }
}
