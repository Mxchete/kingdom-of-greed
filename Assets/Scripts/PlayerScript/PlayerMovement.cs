using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;
using Quaternion = UnityEngine.Quaternion;
using UnityEngine.TextCore;
using Unity.Mathematics;
public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private float moveSpeed = 5f;

    private Vector2 movement;

    private Rigidbody2D rb;
    Animator anim;
    private Vector2 lastMoveDirection;
    
    private bool facingLeft = true;

    public Transform Aim;
    bool isWalking = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

    }
    // Start is called before the first frame update

    // Update is called once per frame - used for inputs and timers
    void Update()
    {
        // Process Input
        ProcessInputs();
        // Animate
        Animate();
        // Flip
        if(movement.x < 0 && !facingLeft || movement.x > 0 && facingLeft)
        {
            Flip();
        }

    }

    // Called once per physics frame - used for physics(used for our movement)
    private void FixedUpdate()
    {
        rb.velocity = movement * moveSpeed;
        if(isWalking)
        {
            Vector3 vector3 = Vector3.left * movement.x + Vector3.down * movement.y;
            Aim.rotation = Quaternion.LookRotation(Vector3.forward, vector3);
        }
        
    }

    void ProcessInputs()
    {
        // Store last move direction when we stop moving
        float InputX  = Input.GetAxisRaw("Horizontal");
        float InputY = Input.GetAxisRaw("Vertical");

        if((InputX == 0 && InputY == 0) && (movement.x != 0 || movement.y != 0) )
        {
            isWalking = false;
            lastMoveDirection = movement;
            Vector3 vector3 = Vector3.left * lastMoveDirection.x + Vector3.down * lastMoveDirection.y;
            Aim.rotation = Quaternion.LookRotation(Vector3.forward, vector3);

        }
        else if(InputX != 0 || InputY != 0)
        {
            isWalking = true;
        }
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        movement.Normalize();

    }

    void Animate()
    {
        anim.SetFloat("InputX", movement.x);
        anim.SetFloat("InputY", movement.y);
        anim.SetFloat("MoveMagnitude",movement.magnitude);
        anim.SetFloat("LastInputX", lastMoveDirection.x);
        anim.SetFloat("LastInputY", lastMoveDirection.y);      
    }

    void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= 1;
        transform.localScale = scale;
        facingLeft = !facingLeft;

    }
}
