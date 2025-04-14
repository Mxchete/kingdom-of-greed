using System.Collections;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] protected float moveSpeed;

    [Header("Health Settings")]
    [SerializeField] protected float health;
    [SerializeField] protected float maxHealth;

    [Header("Death Settings")]
    [SerializeField] protected float deathAnimationDuration = 5f;
    protected bool isDead = false;

    [Header("References")]
    protected Rigidbody2D rb;
    protected SpriteRenderer spriteRenderer;
    protected Animator animator;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        health = maxHealth;
    }

    public virtual void TakeDamage(float damage)
    {
        if(isDead) return; // Prevent taking damage when already dead
        health -= damage;
        StartCoroutine(FlashRed());
        if (health <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        if(isDead) return;

        isDead = true;

        // Disable all colliders
        foreach (var collider in GetComponents<Collider2D>())
        {
            collider.enabled = false;
        }

        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.isKinematic = true;
        }

        if (animator != null)
        {
            animator.SetTrigger("Die");
        }


        StartCoroutine(DestroyAfterDeath());
    }

    private IEnumerator FlashRed()
    {
        Color originalColor = spriteRenderer.color;
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = originalColor;
    }

    protected virtual IEnumerator DestroyAfterDeath()
    {
        // Wait for death animation to complete
        yield return new WaitForSeconds(deathAnimationDuration);

        // Corrected destruction check
        if (this != null && gameObject != null && Application.isPlaying)
        {
            Destroy(gameObject);
        }
    }

}
