using System.Collections;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] protected float moveSpeed;

    [Header("Health Settings")]
    [SerializeField] protected float health;
    [SerializeField] protected float maxHealth;
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
        health -= damage;
        StartCoroutine(FlashRed());
        if (health <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
    }

    private IEnumerator FlashRed()
    {
        Color originalColor = spriteRenderer.color;
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = originalColor;
    }
}
