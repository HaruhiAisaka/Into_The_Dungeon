using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Player Movement")]
    public float speed = 1f;

    // This vector records the direction in which the player is
    //trying to move reguardless of collision events.
    private Vector2 playerDirection;

    public bool freezePlayer = false;

    public int playerHealth;

    [SerializeField] PlayerUI playerUI;

    public BoxCollider2D attackHitBox;

    private Rigidbody2D myRigidBody2D;

    protected Animator myAnimator;
    private bool swordIsVertical;

    // Start is called before the first frame update
    void Awake()
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
        if (!GameState.isPaused()) {
            if(Input.GetButtonDown("Attack1") && !freezePlayer){
                Attack();
            }

            // Makes the Animator Speed 1 by default unless the player's movement is frozen.
            if (!freezePlayer){
                myAnimator.speed = 1;
            }

            if (!freezePlayer){
                Move();
            }
            UpdatePlayerDirection();
        }

        // Pause
        if (Input.GetButtonDown("Pause")) {
            playerUI.TooglePauseState();
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

        // Changes the animation only if there is movement.
        if (deltaX != 0 || deltaY != 0){
            myAnimator.speed = 1;
            myAnimator.SetFloat("deltaX", deltaX);
            myAnimator.SetFloat("deltaY", deltaY);
        }
        // Pauses the animation but still keeps the player in the same direction.
        else {
            myAnimator.speed = 0;
        }

        // Pauses Animation if there is no movement
        if (deltaX == 0 && deltaY == 0){
            myAnimator.speed = 0;
        }
        else {
            myAnimator.speed = 1;
        }
    }

    private void UpdatePlayerDirection(){
        playerDirection = myRigidBody2D.velocity.normalized;
    }

    // Added by Liam R. Forces the player to move to point with speed.
    // Used for animations to move player through doors and stairs.
    public IEnumerator MovePlayerToPoint(Vector2 point, float speed){
        Vector2 initialPlayerPosition = (Vector2) this.transform.position;
        float t = 0f;
        float distance = Vector2.Distance(initialPlayerPosition, point);
        while (Vector2.Distance(this.transform.position, point)> Mathf.Epsilon){
            t += Time.deltaTime;
            this.transform.position =
                Vector2.Lerp(initialPlayerPosition, point, t/(distance/speed));
            yield return null;
        }
    }

    private void Attack(){
        float deltaX = 0;
        float deltaY = 0;
        AnimatorStateInfo state = myAnimator.GetNextAnimatorStateInfo(0);
        myAnimator.speed = 1;
        // set direction
        if (state.IsName("walk_up")) {
            deltaY = 1;
            attackHitBox.transform.rotation = Quaternion.Euler(0,0,90);
        }
        else if (state.IsName("walk_down") ) {
            deltaY = -1;
            attackHitBox.transform.rotation = Quaternion.Euler(0,0,90);
        }
        else if (state.IsName("walk_left") ) {
            deltaX = -1;
        }
        else if (state.IsName("walk_right") ) {
            deltaX = 1;
        }
        // attaaack!
        attackHitBox.transform.position =
            new Vector3(attackHitBox.transform.position.x + deltaX,
            attackHitBox.transform.position.y + deltaY,
            attackHitBox.transform.position.z);
        myAnimator.SetTrigger("action");
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

    public void HealPlayer(int healthCharge){
        playerHealth += healthCharge;

        // Update health in UI, if player has one
        if (playerUI != null) {
            playerUI.UpdateHealth(playerHealth, 14);
        }
    }

    private void AttackEnd(){
        attackHitBox.transform.localPosition = new Vector2(0,0);
        attackHitBox.transform.rotation = Quaternion.identity;
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

    public void FreezeAnimation(){
        myAnimator.speed = 0;
    }

    public void UnfreezeAnimation(){
        myAnimator.speed = 0;
    }

    //Get Functions
    public bool GetFreezePlayer(){
        return freezePlayer;
    }

    public float GetSpeed(){
        return speed;
    }

    public Vector2 GetVelocity(){
        return myRigidBody2D.velocity;
    }

    public Vector2 GetPlayerDirectionVector(){
        return playerDirection;
    }

    public int GetHealth(){
        return playerHealth;
    }
}
