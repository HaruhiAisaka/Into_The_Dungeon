using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class Enemy : MonoBehaviour
{   // Will be called before the first frame update
    public abstract void DerivedStart();
    // function that moves enemy, to be implemented differently by each type of enemy
    public abstract void Move();
    // will be called once per frame
    public abstract void DerivedUpdate();



    protected Rigidbody2D rb;

    protected Animator animator;

    protected SpriteRenderer sprite;

    private bool freezeEnemy = false;
    private Vector2 prevVelocity;

    protected float speed;

    private float stunBlinkSpeed = 0.2f;


    private float health = 10f;
    // Start is called before the first frame update
    void Start()
    {
        sprite = this.GetComponent<SpriteRenderer>();
        rb = this.GetComponent<Rigidbody2D>();
        speed = 3f;
        animator = this.GetComponent<Animator>();
        DerivedStart();

    }

    // Update is called once per frame
    void Update()
    {
        DerivedUpdate();
        Move();
        
        
    }



    // Returns the health of this enemy
    public float GetHealth()
    {
        return health;
    }

    /** Reduces health
    * param damage: amount of health lost */

    public void TakeDamage(float damage)
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Stun"))
        {
            sprite.enabled = false;
            animator.SetTrigger("stun");
            health -= damage;
            if (health < 0f)
            {
                Die();
            }
        }
    }



    // Enemy is destroyed and drops loot
    void Die()
    {
        Destroy(gameObject);
    }

    public void FreezeEnemy()
    {
        freezeEnemy = true;
        prevVelocity = new Vector2(rb.velocity.x, rb.velocity.y);
        rb.velocity = Vector2.zero;
    }

    public void UnfreezeEnemy()
    {
        freezeEnemy = false;
        rb.velocity = prevVelocity;
    }

    public bool GetFreezeEnemy()
    {
        return freezeEnemy;
    }

    private void StunEnd()
    {
        Debug.Log("end stun");
        sprite.enabled = true;
    }

    private void blink(){
        sprite.enabled = !sprite.enabled;
    }
}



