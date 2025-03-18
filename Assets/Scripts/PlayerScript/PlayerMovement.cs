using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;
using Quaternion = UnityEngine.Quaternion;
using UnityEngine.TextCore;
using Unity.Mathematics;
using System.Buffers.Text;
public class PlayerMovement : Entity
{
    private Vector2 movement;
    private Vector2 lastMoveDirection;

    private bool facingLeft = true;

    public Transform Aim;
    bool isWalking = false;

    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();



    }

    // Update is called once per frame - used for inputs and timers
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

    // Called once per physics frame - used for physics(used for our movement)
    private void FixedUpdate()
    {
        rb.velocity = movement * moveSpeed * Time.fixedDeltaTime;

        if (isWalking)
        {

            Vector3 vector3 = Vector3.left * movement.x + Vector3.down * movement.y;
            Aim.rotation = Quaternion.LookRotation(Vector3.forward, vector3);
        }

    }

    void ProcessInputs()
    {
        // Store last move direction when we stop moving
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
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
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


}