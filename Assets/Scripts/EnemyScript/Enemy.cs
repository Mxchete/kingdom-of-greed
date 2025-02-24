using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class Enemy : MonoBehaviour
{

    float health, maxHealth = 3f;
    public float moveSpeed = 2f;
    Rigidbody2D rb;
    Transform target;
    Vector2 moveDirection;

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
        if(target){
            Vector3 direction = (target.position - transform.position).normalized;
            moveDirection = direction;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            rb.rotation = angle;
        }
        
    }

    private void FixedUpdate()
    {
        rb.velocity = new UnityEngine.Vector2(moveDirection.x, moveDirection.y) * moveSpeed;
    }

    public void TakeDamage(float damage){
        health -= damage;

        if(health <= 0){
            Destroy(gameObject);
        }
    }
}
