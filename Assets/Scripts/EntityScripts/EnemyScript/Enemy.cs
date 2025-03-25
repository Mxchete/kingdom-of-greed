using System.Collections;
using UnityEngine;
using UnityEngine.UI;

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

    [Header("References")]
    public LayerMask playerLayer; // LayerMask for detecting the player
    [SerializeField] private FloatingHealthBar healthbar;

    [Header("Health Settings")]
    [SerializeField] private float healthRegenRate = 5f;
    [SerializeField] private float delayBeforeRegen = 2f;
    [SerializeField] private Slider HealthSlider;

    private bool isRegenerating = false;
    private float timeSinceLastDamage = 0f;
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
        if (target == null) return;

        CheckIfStuck();

        float distanceToPlayer = Vector3.Distance(target.position, transform.position);

        // Flee if health is low
        if (health < fleeHealthThreshold)
        {
            Flee();
        }
        // Chase if player is in range
        else if (distanceToPlayer <= detectionRange)
        {
            ChasePlayer();
        }
        // Stop moving if player is out of range
        else
        {
            StopMovement();
        }

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
        Vector3 direction = (target.position - transform.position).normalized;
        moveDirection = direction;

        // Update Animator Parameters for Blend tree
        animator.SetFloat("MoveX", direction.x);
        animator.SetFloat("MoveY", direction.y);
        animator.SetBool("isMoving", true);
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
            healthbar.UpdateHealthBar(health, maxHealth);

            yield return null;
        }

        isRegenerating = false;
        regenCoroutine = null; // Clear Reference
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
        targetHealth = health; // Update target health for smooth interpolation

        healthbar.UpdateHealthBar(health, maxHealth);
        if(regenCoroutine != null){
            StopCoroutine(regenCoroutine);
            regenCoroutine = null;

        }
        isRegenerating = false;
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
        if (Time.time - lastAttackTime >= attackCooldown)
        {
            if (playerHealthComponent != null)
            {
                playerHealthComponent.UpdateHealth(-attackDamage);
                lastAttackTime = Time.time; // Reset the cooldown timer
            }
            else
            {
                Debug.LogError("PlayerHealth component is null!");
            }
        }
    }
}