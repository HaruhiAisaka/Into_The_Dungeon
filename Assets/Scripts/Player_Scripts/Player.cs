using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Player Movement")]
    public float speed = 1f;

    public bool freezePlayer = false;

    public int playerHealth;

    [SerializeField] PlayerUI playerUI;

    public BoxCollider2D attackHitBox;

    private Rigidbody2D myRigidBody2D;

    private Animator myAnimator;
    private bool swordIsVertical;

    // direction of player for use by attack function
    private int directionX = 0;
    private int directionY = -1;

    // Start is called before the first frame update
    void Start()
    {
        attackHitBox.enabled = false;
        myRigidBody2D = this.GetComponent<Rigidbody2D>();
        myAnimator = this.GetComponent<Animator>();
        playerHealth = 10;
        swordIsVertical = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Attack1") && !freezePlayer){
            Attack();
        }
        if (!freezePlayer){
            Move();
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
        // set direction
        if (myAnimator.GetCurrentAnimatorStateInfo(0).IsName("walk_down") ) {
            directionX = 0;
            directionY = -1;
            if (!swordIsVertical) {
                attackHitBox.transform.Rotate(0.0f, 0.0f, 90.0f, Space.Self);
                swordIsVertical = true;
            }
        }
        else if (myAnimator.GetCurrentAnimatorStateInfo(0).IsName("walk_up") ) {
            directionX = 0;
            directionY = 1;
            if (!swordIsVertical) {
                attackHitBox.transform.Rotate(0.0f, 0.0f, 90.0f, Space.Self);
                swordIsVertical = true;
            }
        }
        else if (myAnimator.GetCurrentAnimatorStateInfo(0).IsName("walk_left") ) {
            directionX = -1;
            directionY = 0;
            if (swordIsVertical) {
                attackHitBox.transform.Rotate(0.0f, 0.0f, 90.0f, Space.Self);
                swordIsVertical = false;
            }
        }
        else if (myAnimator.GetCurrentAnimatorStateInfo(0).IsName("walk_right") ) {
            directionX = 1;
            directionY = 0;
            if (swordIsVertical) {
                attackHitBox.transform.Rotate(0.0f, 0.0f, 90.0f, Space.Self);
                swordIsVertical = false;
            }
        }
        // attaaack!
        attackHitBox.transform.position =
            new Vector3(attackHitBox.transform.position.x + directionX,
            attackHitBox.transform.position.y + directionY,
            attackHitBox.transform.position.z);
        myAnimator.SetTrigger("attack");
        attackHitBox.enabled = true;
        FreezePlayer();
    }

    public void DamagePlayer(int damage) {
        playerHealth -= damage;

        // Update health in UI, if player has one
        if (playerUI != null) {
            playerUI.UpdateHealth(playerHealth, 14);
        }

    }
    private void AttackEnd(){
        attackHitBox.transform.position =
            new Vector3(attackHitBox.transform.position.x - directionX,
            attackHitBox.transform.position.y - directionY,
            attackHitBox.transform.position.z);
        attackHitBox.enabled = false;
        UnfreezePlayer();
    }

    //FreezePlayer() Freezes the player in place.
    public void FreezePlayer(){
        freezePlayer = true;
        myRigidBody2D.velocity = new Vector2(0,0);
    }

    //UnfreezePlayer() Allows the player to move again.
    public void UnfreezePlayer(){
        freezePlayer = false;
    }

    //Get Functions
    public bool GetFreezePlayer(){
        return freezePlayer;
    }

    public float GetSpeed(){
        return speed;
    }

    public int GetHealth(){
        return playerHealth;
    }
}
