using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : Entity
{
    private Vector2 movement; // Stores Player's Input Direction
    private Vector2 lastMoveDirection; // Keeps track of the last movement direction

    private bool facingLeft = true; // Tracks if the player is facing left.
    
    [Header("Aim Reference")]
    public Transform Aim;
    bool isWalking = false;
    private bool isAttacking = false;
    private Vector2 attackDirection;

    [Header("Current Weapon Reference")]
    public GameObject currentWeapon; // Reference to the current weapon


    protected override void Awake()
    {
        base.Awake();
        /* Inside Parent Class:
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        health = maxHealth;
        */

        // Already Declared in the Parent Class, Now initializing with the Player's animator
        animator = GetComponent<Animator>();
    }

    // Method Unique to PlayerMovement
    private void Update()
    {
        // Process Input
        ProcessInputs();

        // Animate
        Animate();

        // Flip
        if (movement.x < 0 && !facingLeft || movement.x > 0 && facingLeft)
        {
            Flip();
        }
    }

    // Method that ensures certain things are Fixed Updated
    private void FixedUpdate()
    {
        if(isAttacking){
            rb.velocity = Vector2.zero;
            return;
        }
        // Moves the Player using physics
        rb.velocity = movement * moveSpeed;

        // Rotates the aim direction while walking.
        if (isWalking)
        {
            Vector3 vector3 = Vector3.left * movement.x + Vector3.down * movement.y;
            Aim.rotation = Quaternion.LookRotation(Vector3.forward, vector3);
        }
    }

    // Method for Processing Input from Users
    void ProcessInputs()
    {

        float InputX = Input.GetAxisRaw("Horizontal");
        float InputY = Input.GetAxisRaw("Vertical");
        
        // If input stops, save the last direction and stop walking.
        if ((InputX == 0 && InputY == 0) && (movement.x != 0 || movement.y != 0))
        {
            isWalking = false;
            lastMoveDirection = movement;

            // Update the aim rotation to last direction
            Vector3 vector3 = Vector3.left * lastMoveDirection.x + Vector3.down * lastMoveDirection.y;
            Aim.rotation = Quaternion.LookRotation(Vector3.forward, vector3);
        }
        else if (InputX != 0 || InputY != 0)
        {
            isWalking = true;
        }

        movement.x = InputX;
        movement.y = InputY;
        movement.Normalize(); // Ensures diagonal movement isn;t faster.
    }

    void Animate()
    {
        // Passing movement values to the animator for Blend Trees
        animator.SetFloat("InputX", movement.x);
        animator.SetFloat("InputY", movement.y);

        // Controls idle and walk transitions.
        animator.SetFloat("MoveMagnitude", movement.magnitude);

        // Ensures Idle and walk animations face the correct direction
        animator.SetFloat("LastInputX", lastMoveDirection.x);
        animator.SetFloat("LastInputY", lastMoveDirection.y);
    }

    void Flip()
    {
        facingLeft = !facingLeft;
        Vector3 scale = transform.localScale;
        scale.x *= 1; // Flipts the sprite horizontally.
        transform.localScale = scale;
    }

    // Method for Triggering the Attack Animations
    public void TriggerAttackAnimation()
    {
        if(movement.magnitude > 0.1f){
            lastMoveDirection = movement;
            attackDirection = movement;
        }

        // Forces animator to use last direction.
        animator.SetFloat("LastInputX", lastMoveDirection.x);
        animator.SetFloat("LastInputY", lastMoveDirection.y);

        // Stops movement during attack
        animator.SetFloat("MoveMagnitude", 0f);

        isAttacking = true;
        // Trigger the attack animation after saving last movement direction
        animator.SetTrigger("AttackTrigger");

        isAttacking = false;

    }

    public void EndAttack(){
        isAttacking = false;
    }
}