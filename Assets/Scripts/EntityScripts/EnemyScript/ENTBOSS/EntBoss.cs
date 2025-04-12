using System.Collections;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UI;

public class EntBoss : Entity
{

    [Header("Ent Boss's Health Manager")]
    [SerializeField] private Slider HealthSlider;

    private float timeSinceLastDamage = 0f;
    [Header("Detection Settings")]
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

    private Transform player;
    private float attackCooldown;
    private bool isAttacking = false;
    private float recoveryTime = 0.5f; // Added recovery time

    protected override void Awake()
    {
        base.Awake();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        HealthSlider.maxValue = maxHealth;
        HealthSlider.value = health;

    }

    public void UpdateHealth(float mod)
    {
        health += mod;

        if (health > maxHealth)
        {
            health = maxHealth;
        }
        else if (health <= 0)
        {
            health = 0f;
            HealthSlider.value = 0f;
            //Will Add the animation of dying once done/given
            Die();
        }
    }

    private void OnGUI()
    {
        float t = Time.deltaTime / 0.5f;
        HealthSlider.value = Mathf.Lerp(HealthSlider.value, health, t);
    }

    private void Update()
    {
        if (player == null)
        {
            Debug.LogError("Player reference is null!");
            return;
        }

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // Debug each condition separately
        // Debug.Log($"Distance: {distanceToPlayer} <= {attackRange}? {distanceToPlayer <= attackRange}");
        Debug.Log($"Cooldown: {attackCooldown} <= 0? {attackCooldown <= 0}");
        Debug.Log($"Not Attacking? {!isAttacking}");

        if (distanceToPlayer <= attackRange && attackCooldown <= 0 && !isAttacking)
        {
            Debug.Log("ALL CONDITIONS MET - STARTING ATTACK");
            StartCoroutine(StompAttack());
            attackCooldown = timeBetweenAttacks;
        }
    }

    private void FacePlayer()
    {
        if ((player.position.x > transform.position.x && transform.localScale.x < 0) ||
            (player.position.x < transform.position.x && transform.localScale.x > 0))
        {
            transform.localScale = new Vector3(-transform.localScale.x,
                                            transform.localScale.y,
                                            transform.localScale.z);
        }
    }

    private void MoveTowardsPlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        rb.velocity = new Vector2(direction.x * moveSpeed, rb.velocity.y);
    }

    private IEnumerator StompAttack()
    {
        Debug.Log("=== STARTING STOMP TEST ===");


        // 2. Force the animation to play
        animator.Play("Stomp", -1, 0f);
        Debug.Log("Forced animation play attempt");
        yield return null; // Wait one frame

        // 3. Verify
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Stomp"))
        {
            Debug.Log("SUCCESS: Animation is playing!");
        }
        else
        {
            Debug.LogError("FAILURE: Animation not playing. Check:");
            Debug.LogError("- State name spelling");
            Debug.LogError("- Animation assignment");
        }

    }

    // Can be called from animation event for perfect timing
    public void OnStompImpact()
    {
        ExecuteStompImpact();
    }

    private void ExecuteStompImpact()
    {
        // Visual Feedback
        if (stompParticles != null)
        {
            Instantiate(stompParticles, transform.position, Quaternion.identity);
        }

        // Damage Application
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, stompRadius, playerLayer);
        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                hit.GetComponent<Player>().TakeDamage(stompDamage);

                // Knockback
                Vector2 direction = (hit.transform.position - transform.position).normalized;
                hit.GetComponent<Rigidbody2D>().AddForce(direction * knockbackForce, ForceMode2D.Impulse);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Detection range (yellow)
        Gizmos.color = new Color(1, 1, 0, 0.2f);
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        // Attack range (red)
        Gizmos.color = new Color(1, 0, 0, 0.2f);
        Gizmos.DrawWireSphere(transform.position, attackRange);

        // Stomp radius (solid red)
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawWireSphere(transform.position, stompRadius);
    }

    // In EntBoss.cs
    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage); // Calls Entity's TakeDamage

        // Update UI
        if (HealthSlider != null)
        {
            HealthSlider.value = health;
        }

        Debug.Log($"Boss took {damage} damage. Health: {health}"); // Debug
    }

    protected override void Die()
    {
        // Add any boss-specific death logic here
        if (HealthSlider != null)
        {
            HealthSlider.value = 0f;
        }
        base.Die(); // Calls Entity's Die
    }
}