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

    float health, maxHealth = 3f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.Find("Player").transform;
        health = maxHealth;
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
            }
            else
            {
                // Stop moving if the player is out of range
                moveDirection = Vector2.zero;
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

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
