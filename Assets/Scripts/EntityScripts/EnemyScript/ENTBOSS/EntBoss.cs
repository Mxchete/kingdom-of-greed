using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EntBoss : Entity
{
    [Header("Health Settings")]
    [SerializeField] private Slider healthSlider;

    [Header("Movement Settings")]
    [SerializeField] private float detectionRange = 20f;
    [SerializeField] private float attackRange = 15f;
    [SerializeField] private float stoppingDistance = 10f;

    [Header("Attack Settings")]
    [SerializeField] private float stompDamage = 15f;
    [SerializeField] private float stompRadius = 10f;
    [SerializeField] private float timeBetweenAttacks = 3f;
    [SerializeField] private LayerMask playerLayer;

    [Header("Visual Effects")]
    [SerializeField] private ParticleSystem stompParticles;
    [SerializeField] private GameObject deathEffect;
    [SerializeField] private AudioClip stompSound;

    [Header("Vine Attack Settings")]
    [SerializeField] private int vineCount = 20;          // Number of vines to spawn
    [SerializeField] private float vineRadius = 15f;      // Distance from boss
    [SerializeField] private float vineOffsetAngle = 20f; // Angle variation for randomness
    [SerializeField] private GameObject vinePrefab;

    private bool hasDetectedPlayer = false;
    private bool isLaughing = false;

    private int StompCount = 0;


    // Other
    private Transform player;
    private float attackCooldown = 3f;
    private bool isAttacking = false;
    private bool isMoving = false;
    private const float recoveryTime = 5f;

    protected override void Awake()
    {
        base.Awake();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = health;
        }
    }

    private void Update()
    {
        if (player == null || isDead) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // Cooldown handling
        if (attackCooldown > 0)
        {
            attackCooldown -= Time.deltaTime;
        }

        if (distanceToPlayer < detectionRange && !hasDetectedPlayer)
        {
            hasDetectedPlayer = true;
            isLaughing = true;
            StartCoroutine(InitialLaugh());
            return;
        }

        if (isLaughing) return;

        // Movement
        if (distanceToPlayer <= detectionRange && distanceToPlayer > stoppingDistance)
        {
            MoveTowardsPlayer();
            isMoving = true;
        }
        else
        {
            StopMovement();
            isMoving = false;
        }

        // Attack logic
        if (distanceToPlayer <= attackRange && attackCooldown <= 0 && !isAttacking)
        {
            StartCoroutine(StompAttack());
            StompCount++;
            if (StompCount == 2 % 1){
                VineLaughAttack();
            }
        }

        animator.SetBool("isMoving", isMoving);
    }

    private IEnumerator InitialLaugh()
    {
        // Immediately trigger animation and skip any transition delay
        animator.Play("LaughIntro", -1, 0f); // Force play from start
        animator.SetBool("PlayerDetected", true);

        // Wait for animation length instead of fixed duration
        yield return new WaitForSeconds(4.5f);

        animator.SetBool("PlayerDetected", false);

        // Play vine attack animation
        animator.SetTrigger("VineLaugh");

        yield return new WaitForSeconds(4f);
        isLaughing = false;
    }

    private IEnumerator StompAttack()
    {
        isAttacking = true;
        attackCooldown = timeBetweenAttacks;
        animator.SetTrigger("Stomp");
        Debug.Log("Attempting to deal stomp damage!");
        yield return new WaitForSeconds(2f);
        ExecuteStompImpact();
        Debug.Log("Damage dealt!");
        yield return new WaitForSeconds(recoveryTime); // Wait for recovery
        isAttacking = false;
    }

    public void ExecuteStompImpact()
    {
        // Visual/Audio effects
        if (stompParticles) Instantiate(stompParticles, transform.position, Quaternion.identity);
        if (stompSound) AudioSource.PlayClipAtPoint(stompSound, transform.position);

        // Damage players in radius
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, stompRadius, playerLayer);
        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Player") && hit.TryGetComponent<Player>(out var playerComponent))
            {
                Vector2 direction = (hit.transform.position - transform.position).normalized;
                playerComponent.TakeDamage(stompDamage, direction);
            }
        }
    }

    private IEnumerator VineLaughAttack()
    {
        isLaughing = true;
        animator.SetTrigger("VineLaugh");
        yield return new WaitForSeconds(4f);
        SpawnVines();
        isLaughing = false;
    }

    private void MoveTowardsPlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        rb.velocity = new Vector2(direction.x * moveSpeed, rb.velocity.y);
    }

    private void StopMovement()
    {
        rb.velocity = new Vector2(0, rb.velocity.y);
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        healthSlider.value = health;
    }

    protected override void Die()
    {
        if (deathEffect != null)
            Instantiate(deathEffect, transform.position, Quaternion.identity);

        base.Die();
    }

    private void OnDrawGizmosSelected()
    {
        // Detection range
        Gizmos.color = new Color(1, 1, 0, 0.1f);
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        // Attack range
        Gizmos.color = new Color(1, 0, 0, 0.1f);
        Gizmos.DrawWireSphere(transform.position, attackRange);

        // Stomp radius
        Gizmos.color = new Color(1, 0, 1, 0.3f);
        Gizmos.DrawWireSphere(transform.position, stompRadius);

    
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, vineRadius);
    }

    // Attach this to the boss to debug collisions
    public class BossCollisionDebug : MonoBehaviour
    {
        void OnCollisionEnter2D(Collision2D collision)
        {
            Debug.Log($"Boss collided with: {collision.gameObject.name}");
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log($"Boss trigger entered by: {other.gameObject.name}");
        }
    }
/*
    public void SpawnVines()
    {
        float angleStep = 360f / vineCount;
        int numberOfRings = 5; // How many concentric rings you want
        float finalRadius = 15f; // Your specified final radius
        float delayBetweenRings = 0.15f; // Time between each ring

        StartCoroutine(SpawnVinesWave(angleStep, numberOfRings, finalRadius, delayBetweenRings));
    }

    private IEnumerator SpawnVinesWave(float angleStep, int rings, float finalRadius, float delay)
    {
        for (int currentRing = 1; currentRing <= rings; currentRing++)
        {
            // Calculate current radius (linearly increasing to final radius)
            float currentRadius = (finalRadius * currentRing) / rings;

            // Calculate how many vines to spawn in this ring
            int vinesThisRing = Mathf.RoundToInt(vineCount * (currentRing / (float)rings));

            for (int i = 0; i < vinesThisRing; i++)
            {
                float currentAngle = i * (360f / vinesThisRing);
                float randomOffset = Random.Range(-vineOffsetAngle, vineOffsetAngle);
                Vector2 direction = Quaternion.Euler(0, 0, currentAngle + randomOffset) * Vector2.right;
                Vector2 spawnPos = (Vector2)transform.position + direction * currentRadius;

                GameObject vine = Instantiate(vinePrefab, spawnPos, Quaternion.identity);
                vine.transform.up = direction;

                // If your prefab has animation, trigger it here
                if (vine.TryGetComponent<Animator>(out var anim))
                {
                    anim.SetTrigger("Grow");
                }
            }

            yield return new WaitForSeconds(delay);
        }
    }*/

    public void SpawnVines()
    {
        float angleStep = 360f / vineCount;
        float radiusIncrement = vineRadius / vineCount; // Distance between each vine ring
        float delayBetweenRings = 0.2f; // Time between each ring of vines

        StartCoroutine(SpawnVinesWave(angleStep, radiusIncrement, delayBetweenRings));
    }

    private IEnumerator SpawnVinesWave(float angleStep, float radiusIncrement, float delay)
    {
        for (int ring = 0; ring < vineCount; ring++)
        {
            float currentRadius = ring * radiusIncrement;

            for (int i = 0; i < vineCount; i++)
            {
                float currentAngle = i * angleStep;
                float randomOffset = Random.Range(-vineOffsetAngle, vineOffsetAngle);
                Vector2 direction = Quaternion.Euler(0, 0, currentAngle + randomOffset) * Vector2.right;
                Vector2 spawnPos = (Vector2)transform.position + direction * currentRadius;

                GameObject vine = Instantiate(vinePrefab, spawnPos, Quaternion.identity);
                vine.transform.up = direction;
            }

            yield return new WaitForSeconds(delay);
        }
    }
    /*public void SpawnVines()
    {
        float angleStep = 360f / vineCount;
        float currentAngle = 0f;

        for (int i = 0; i < vineCount; i++)
        {
            // Calculate position in a circle
            Vector2 spawnDir = Quaternion.Euler(0, 0, currentAngle) * Vector2.right;
            Vector2 spawnPos = (Vector2)transform.position + spawnDir * vineRadius;

            // Add slight randomness to angles
            float randomOffset = Random.Range(-vineOffsetAngle, vineOffsetAngle);
            Vector2 finalDir = Quaternion.Euler(0, 0, randomOffset) * spawnDir;
            Vector2 finalPos = (Vector2)transform.position + finalDir * vineRadius;

            // Spawn and orient vine outward
            GameObject vine = Instantiate(vinePrefab, finalPos, Quaternion.identity);
            vine.transform.up = finalDir; // Make vines point outward

            currentAngle += angleStep;
        }
    }*/
}