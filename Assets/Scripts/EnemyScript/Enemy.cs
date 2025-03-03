using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class Enemy : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float detectionRange = 10f; // Distance within which the enemy will start following the player

    Rigidbody2D rb;
    Transform target;
    Vector2 moveDirection;

    [SerializeField] float health;
    [SerializeField] float maxHealth = 3f;

    [SerializeField] FloatingHealthBar healthbar;
    private SpriteRenderer spriteRenderer;
    private Animator animator; // The Animator component

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        healthbar = GetComponentInChildren<FloatingHealthBar>();

    }

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.Find("Player").transform;
        health = maxHealth;
        healthbar.UpdateHealthBar(health, maxHealth);
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (target)
        {
            float distanceToPlayer = Vector3.Distance(target.position, transform.position);

            // Check if the player is within the detection range
            if (distanceToPlayer <= detectionRange)
            {
                Vector3 direction = (target.position - transform.position).normalized;
                moveDirection = direction;

                // float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                // rb.rotation = angle;

                // Update Animator Parameters for Blend tree
                animator.SetFloat("MoveX", direction.x);
                animator.SetFloat("MoveY", direction.y);

                animator.SetBool("isMoving", moveDirection.magnitude > 0.1f);

            }
            else
            {
                // Stop moving if the player is out of range
                moveDirection = Vector2.zero;
                animator.SetBool("isMoving", false);

            }
        }
    }

    private void FixedUpdate()
    {
        if (target)
        {
            rb.velocity = new Vector2(moveDirection.x, moveDirection.y) * moveSpeed;
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        healthbar.UpdateHealthBar(health, maxHealth);

        // Flash Red when Attacked
        StartCoroutine(FlashRed());

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator FlashRed()
    {
        Color originalColor = spriteRenderer.color; // Store the original color of the sprite
        spriteRenderer.color = Color.red; // Change to red when hit
        yield return new WaitForSeconds(0.1f); // Flash duration
        spriteRenderer.color = originalColor;  // Restore original color

    }
}
