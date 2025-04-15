using System.Collections;
using System.Numerics;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class Enemy : Entity
{
    [Header("Movement Settings")]
    public float detectionRange = 10f; // Distance within which the enemy will start following the player
    public float fleeHealthThreshold = 5f; // Health below which the enemy will flee

    [Header("Obstacle Avoidance")]
    public float avoidanceDistance = 2f; // Distance to check for obstacles
    public LayerMask obstacleLayer; // LayerMask for obstacles

    [Header("Stuck Detection")]
    public float stuckTimeThreshold = 2f; // Time after which the enemy is considered stuck
    public float randomMovementForce = 1f; // Force to apply for random movement
    public float stuckDistanceThreshold = 0.1f; // Distance below which the enemy is considered stuck

    [Header("Repulsion")]
    public float repulsionForce = 5f; // Force to push enemies apart
    public float repulsionRadius = 2f; // Radius within which enemies repel each other

    [Header("Attack Settings")]
    public int attackDamage = 5; // Reduced damage
    public float attackCooldown = 1f; // Time between attacks
    public float attackRange = 1.5f; // Range at which enemy can attack
    public float attackWindup = 0.3f; // Time before damage is dealt
    private bool isAttacking = false;


    [Header("References")]
    public LayerMask playerLayer; // LayerMask for detecting the player
    [SerializeField] private FloatingHealthBar healthbar;

    [Header("Health Settings")]
    [SerializeField] private float healthRegenRate = 5f;
    [SerializeField] private float delayBeforeRegen = 2f;
    [SerializeField] private Slider HealthSlider;

    private bool isRegenerating = false;
    private float targetHealth; // Target health value for smooth interpolation

    private Transform target;
    private Vector2 moveDirection;
    private playerHealth playerHealthComponent;
    private float stuckTimer = 0f;
    private Vector3 lastPosition;
    private float lastAttackTime = 0f; // Time of the last attack

    private Coroutine regenCoroutine; // Store the coroutine reference

    protected override void Awake()
    {
        base.Awake();
        healthbar = GetComponentInChildren<FloatingHealthBar>();

        if(healthbar == null){
            Debug.LogError("FloatingHealthBar component missing!");
        }

        targetHealth = health; // Initialize target health
    }

    private void Start()
    {
        target = GameObject.FindWithTag("Player").transform;
        if (target != null)
        {
            playerHealthComponent = target.GetComponent<playerHealth>();
            if (playerHealthComponent == null)
            {
                Debug.LogError("PlayerHealth component not found on the player!");
            }
        }
        else
        {
            Debug.LogError("Player target not found!");
        }
        healthbar.UpdateHealthBar(health, maxHealth);
        lastPosition = transform.position;
    }

    private void Update()
    {
        if(isDead) return;
        
        if (target == null) return;

        CheckIfStuck();

        float distanceToPlayer = Vector3.Distance(target.position, transform.position);

        // Flee if health is low
        if (health < fleeHealthThreshold)
        {
            Debug.Log("Enemy Running Away");
            Flee();
        }
        // Chase if player is in range but not in attack range
        else if (distanceToPlayer <= detectionRange && distanceToPlayer > attackRange)
        {
            ChasePlayer();
        }
        // Attack if player is in attack range
        else if (distanceToPlayer <= attackRange && !isAttacking && Time.time - lastAttackTime >= attackCooldown)
        {
            Debug.Log("Player in range, Enemy will start attacking");
            StartCoroutine(Attack());
        }
        // Stop moving if player is out of range
        else
        {
            StopMovement();
        }
    }

    private IEnumerator Attack()
    {
        isAttacking = true;
        lastAttackTime = Time.time;

        // Set attacking state in animator
        animator.SetBool("isAttacking", true);

        //Determine the attack direction
        Vector2 attackDirection = (target.position - transform.position).normalized;

        // Set animator parameters
        animator.SetFloat("AttackX", attackDirection.x);
        animator.SetFloat("AttackY", attackDirection.y);

        // Stop movement during attack
        StopMovement();

        // Windup time before damage is dealt
        yield return new WaitForSeconds(attackWindup);

        // Check if player is still in range
        if (Vector3.Distance(target.position, transform.position) <= attackRange && playerHealthComponent != null)
        {
            playerHealthComponent.UpdateHealth(-attackDamage);
        }

        // End attack animation
        animator.SetBool("isAttacking", false);
        isAttacking = false;
    }

    private void FixedUpdate()
    {
        if (target != null)
        {
            AvoidObstacles();
            RepelOtherEnemies();
            rb.velocity = moveDirection * moveSpeed;
        }
    }

    private void ChasePlayer()
    {
        float distanceToPlayer = Vector3.Distance(target.position, transform.position);

        if (distanceToPlayer > attackRange)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            moveDirection = direction;

            // Update Animator Parameters for Blend tree
            animator.SetFloat("MoveX", direction.x);
            animator.SetFloat("MoveY", direction.y);
            animator.SetBool("isMoving", true);
        }
        else
        {
            StopMovement();
        }
    }

    private void Flee()
    {
        Vector3 directionAwayFromPlayer = (transform.position - target.position).normalized;
        moveDirection = directionAwayFromPlayer;

        // Update Animator Parameters for Blend tree
        animator.SetFloat("MoveX", directionAwayFromPlayer.x);
        animator.SetFloat("MoveY", directionAwayFromPlayer.y);
        animator.SetBool("isMoving", true);

        if (!isRegenerating)
        {
            StartCoroutine(RegenerateHealth());
        }
    }

    private IEnumerator RegenerateHealth()
    {
        isRegenerating = true;
        yield return new WaitForSeconds(delayBeforeRegen);

        while (health < maxHealth)
        {
            health += healthRegenRate * Time.deltaTime;
            health = Mathf.Min(health, maxHealth);

            targetHealth = health;

            // Check if healthbar exists and is active before updating
            if (healthbar != null && healthbar.gameObject.activeInHierarchy)
            {
                healthbar.UpdateHealthBar(health, maxHealth);
            }

            yield return null;
        }

        isRegenerating = false;
        regenCoroutine = null;
    }

    private void StopMovement()
    {
        moveDirection = Vector2.zero;
        rb.velocity = Vector2.zero;
        animator.SetBool("isMoving", false);
    }

    private void AvoidObstacles()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, moveDirection, avoidanceDistance, obstacleLayer);
        if (hit.collider != null)
        {
            // Change direction to avoid the obstacle
            moveDirection = Vector2.Perpendicular(hit.normal).normalized;
        }
    }

    private void RepelOtherEnemies()
    {
        Collider2D[] nearbyEnemies = Physics2D.OverlapCircleAll(transform.position, repulsionRadius, obstacleLayer);
        foreach (var enemy in nearbyEnemies)
        {
            if (enemy.gameObject != gameObject)
            {
                Vector2 direction = (transform.position - enemy.transform.position).normalized;
                rb.AddForce(direction * repulsionForce, ForceMode2D.Force);
            }
        }
    }

    private void CheckIfStuck()
    {
        if (Vector3.Distance(transform.position, lastPosition) < stuckDistanceThreshold)
        {

            Debug.Log("Enemy is stuck, applying unstucking function");
            stuckTimer += Time.deltaTime;
            if (stuckTimer >= stuckTimeThreshold)
            {
                ApplyRandomMovement();
                stuckTimer = 0f;
            }
        }
        else
        {
            stuckTimer = 0f;
        }
        lastPosition = transform.position;
    }

    private void ApplyRandomMovement()
    {
        Vector2 randomDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
        rb.AddForce(randomDirection * randomMovementForce, ForceMode2D.Impulse);
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        targetHealth = health;

        // Check if healthbar exists and is active before updating
        if (healthbar != null && healthbar.gameObject.activeInHierarchy)
        {
            healthbar.UpdateHealthBar(health, maxHealth);
        }

        if (regenCoroutine != null)
        {
            StopCoroutine(regenCoroutine);
            regenCoroutine = null;
            PlayerStats.Instance.KillEnt();
        }
        isRegenerating = false;
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }


    protected override void Die()
    {
        if (isDead) return;

        // Stop all enemy behavior first
        StopAllCoroutines();
        StopMovement();

        // Disable healthbar if it exists
        if (healthbar != null)
        {
            healthbar.gameObject.SetActive(false);
        }

        // Disable the Enemy script components
        this.enabled = false;

        // Call base Die() to handle the actual death sequence
        base.Die();
    }

}