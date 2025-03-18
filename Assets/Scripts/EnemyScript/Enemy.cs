using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class Enemy : Entity
{
    public float detectionRange = 10f; // Distance within which the enemy will start following the player
    Transform target;
    Vector2 moveDirection;

    [SerializeField] FloatingHealthBar healthbar;

    [SerializeField] private float attackDamage = 10f;

    [SerializeField] private float attackSpeed = 1f;

    private float canAttack;
    protected override void Awake()
    {
        base.Awake();
        healthbar = GetComponentInChildren<FloatingHealthBar>();

    }

    // Start is called before the first frame update
    private void Start()
    {
        target = GameObject.Find("Player").transform;
        healthbar.UpdateHealthBar(health, maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        if (target)
        {
            float distanceToPlayer = Vector3.Distance(target.position, transform.position);

            // Check if the player is within the detection range
            if (distanceToPlayer <= detectionRange && !animator.GetCurrentAnimatorStateInfo(0).IsName("DownAttack"))
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

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (attackSpeed <= canAttack)
            {
                other.gameObject.GetComponent<playerHealth>().UpdateHealth(-attackDamage);
                canAttack = 0f;

                // trigger attack Animation
                animator.SetTrigger("Attack");
            }
            else
            {
                canAttack += Time.deltaTime;
            }
        }

    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        healthbar.UpdateHealthBar(health, maxHealth);
    }
}

