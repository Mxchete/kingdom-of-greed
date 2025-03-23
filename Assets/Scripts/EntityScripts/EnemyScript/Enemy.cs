using UnityEngine;

public class Enemy : Entity
{
    public float detectionRange = 10f; // Distance within which the enemy will start following the player
    public float attackDamage = 10f;
    public float attackSpeed = 1f;
    public LayerMask playerLayer; // LayerMask for detecting the player

    private Transform target;
    private Vector2 moveDirection;
    private float canAttack;
    private playerHealth playerHealthComponent;

    [SerializeField] private FloatingHealthBar healthbar;

    protected override void Awake()
    {
        base.Awake();
        healthbar = GetComponentInChildren<FloatingHealthBar>();
    }

    private void Start()
    {
        target = GameObject.FindWithTag("Player").transform;
        if (target != null)
        {
            playerHealthComponent = target.GetComponent<playerHealth>();
        }
        healthbar.UpdateHealthBar(health, maxHealth);
    }

    private void Update()
    {
        if (target == null) return;

        float distanceToPlayer = Vector3.Distance(target.position, transform.position);

        // Check if the player is within the detection range
        if (distanceToPlayer <= detectionRange && !animator.GetCurrentAnimatorStateInfo(0).IsName("DownAttack"))
        {
            Vector3 direction = (target.position - transform.position).normalized;
            moveDirection = direction;

            // Update Animator Parameters for Blend tree
            animator.SetFloat("MoveX", direction.x);
            animator.SetFloat("MoveY", direction.y);
            animator.SetBool("isMoving", moveDirection.magnitude > 0.1f);
        }
        else
        {
            StopMovement();
        }
    }

    private void FixedUpdate()
    {
        if (target != null)
        {
            rb.velocity = new Vector2(moveDirection.x, moveDirection.y) * moveSpeed;
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            AttackPlayer();
        }
    }

    private void AttackPlayer()
    {
        if (attackSpeed <= canAttack && playerHealthComponent != null)
        {
            playerHealthComponent.UpdateHealth(-attackDamage);
            canAttack = 0f;

            // Trigger attack animation
            animator.SetTrigger("Attack");
        }
        else
        {
            canAttack += Time.deltaTime;
        }
    }

    private void StopMovement()
    {
        moveDirection = Vector2.zero;
        rb.velocity = Vector2.zero;
        animator.SetBool("isMoving", false);
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        healthbar.UpdateHealthBar(health, maxHealth);
    }
}