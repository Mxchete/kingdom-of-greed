using System.Collections;
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
    [SerializeField] private float stompRadius = 4f;
    [SerializeField] private float timeBetweenAttacks = 1f;
    [SerializeField] private float knockbackForce = 5f;
    [SerializeField] private LayerMask playerLayer;

    [Header("Visual Effects")]
    [SerializeField] private ParticleSystem stompParticles;
    [SerializeField] private GameObject deathEffect;
    [SerializeField] private AudioClip stompSound;

    private Transform player;
    private float attackCooldown;
    private bool isAttacking = false;
    private bool isMoving = false;
    private const float recoveryTime = 0.5f;

    protected override void Awake()
    {
        base.Awake();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        healthSlider.maxValue = maxHealth;
        healthSlider.value = health;
    }

    private void Update()
    {
        if (player == null || isDead) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

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
            attackCooldown = timeBetweenAttacks;
        }
        else if (attackCooldown > 0)
        {
            attackCooldown -= Time.deltaTime;
        }

        // Update animator
        animator.SetBool("isMoving", isMoving);
    }

    private IEnumerator StompAttack()
    {
        isAttacking = true;
        animator.SetTrigger("Stomp");

        // Wait until attack frame (adjust 0.4 based on your animation)
        yield return new WaitUntil(() =>
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.4f);

        ExecuteStompImpact();

        // Complete animation
        yield return new WaitWhile(() =>
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f);

        yield return new WaitForSeconds(recoveryTime);
        isAttacking = false;
    }

    private void ExecuteStompImpact()
    {
        // Visual/Audio effects
        if (stompParticles != null)
            Instantiate(stompParticles, transform.position, Quaternion.identity);

        if (stompSound != null)
            AudioSource.PlayClipAtPoint(stompSound, transform.position);

        // Damage players in radius
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, stompRadius, playerLayer);
        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Player") && hit.TryGetComponent<Player>(out var playerComponent))
            {
                playerComponent.TakeDamage(stompDamage);

                // Knockback
                if (hit.TryGetComponent<Rigidbody2D>(out var rb))
                {
                    Vector2 direction = (hit.transform.position - transform.position).normalized;
                    direction = new Vector2(direction.x, Mathf.Clamp(direction.y, -0.5f, 0.5f)).normalized;
                    rb.AddForce(direction * knockbackForce, ForceMode2D.Impulse);
                }
            }
        }
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
    }
}