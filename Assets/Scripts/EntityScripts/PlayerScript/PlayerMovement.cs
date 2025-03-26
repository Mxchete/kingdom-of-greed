using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : Entity
{
    private Vector2 movement;
    private Vector2 lastMoveDirection;

    private bool facingLeft = true;
    
    [Header("Aim Reference")]
    public Transform Aim;
    bool isWalking = false;

    [Header("Current Weapon Reference")]
    public GameObject currentWeapon; // Reference to the current weapon

    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
    }

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

    private void FixedUpdate()
    {
        // Apply movement directly to velocity
        rb.velocity = movement * moveSpeed;

        if (isWalking)
        {
            Vector3 vector3 = Vector3.left * movement.x + Vector3.down * movement.y;
            Aim.rotation = Quaternion.LookRotation(Vector3.forward, vector3);
        }
    }

    void ProcessInputs()
    {
        float InputX = Input.GetAxisRaw("Horizontal");
        float InputY = Input.GetAxisRaw("Vertical");

        if ((InputX == 0 && InputY == 0) && (movement.x != 0 || movement.y != 0))
        {
            isWalking = false;
            lastMoveDirection = movement;
            Vector3 vector3 = Vector3.left * lastMoveDirection.x + Vector3.down * lastMoveDirection.y;
            Aim.rotation = Quaternion.LookRotation(Vector3.forward, vector3);
        }
        else if (InputX != 0 || InputY != 0)
        {
            isWalking = true;
        }

        movement.x = InputX;
        movement.y = InputY;
        movement.Normalize();
    }

    void Animate()
    {
        animator.SetFloat("InputX", movement.x);
        animator.SetFloat("InputY", movement.y);
        animator.SetFloat("MoveMagnitude", movement.magnitude);
        animator.SetFloat("LastInputX", lastMoveDirection.x);
        animator.SetFloat("LastInputY", lastMoveDirection.y);
    }

    void Flip()
    {
        facingLeft = !facingLeft;
        Vector3 scale = transform.localScale;
        scale.x *= 1;
        transform.localScale = scale;
    }
    public void TriggerAttackAnimation()
    {
        if(movement.magnitude > 0.1f){
            lastMoveDirection = movement;
        }

        animator.SetFloat("InputX", lastMoveDirection.x);
        animator.SetFloat("InputY", lastMoveDirection.y);

        animator.SetFloat("MoveMagnitude", 0.01f);

        // Trigger the attack animation after saving last movement direction
        animator.SetTrigger("AttackTrigger");

    }
}