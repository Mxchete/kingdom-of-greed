using System.Collections;
using UnityEngine;

public class VineController : MonoBehaviour
{
    [Header("Damage Settings")]
    [SerializeField] private float damage = 10f;
    [SerializeField] private float activeTime = 0.5f;
    [SerializeField] private bool preventMultiHits = true;

    [Header("Visual Settings")]
    [SerializeField] private float growDuration = 0.3f;
    [SerializeField] private ParticleSystem emergeParticles;
    [SerializeField] private ParticleSystem retractParticles;

    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Collider2D vineCollider;
    private bool canDamage = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        vineCollider = GetComponent<Collider2D>();

        InitializeVine();
    }

    private void InitializeVine()
    {
        // Start disabled
        spriteRenderer.enabled = false;
        if (vineCollider != null) vineCollider.enabled = false;
    }

    public void Initialize(float damageAmount)
    {
        damage = damageAmount;
        animator.SetTrigger("Grow");
        StartCoroutine(VineLifecycle());
    }

    private IEnumerator VineLifecycle()
    {
        // Initial delay (optional)
        yield return new WaitForSeconds(0.1f);

        // Enable renderer and play grow animation
        spriteRenderer.enabled = true;
        animator.SetTrigger("Grow");

        // Play emerge particles
        if (emergeParticles != null)
        {
            emergeParticles.Play();
        }

        // Wait for growth to complete
        yield return new WaitForSeconds(growDuration);

        // Enable damage
        canDamage = true;
        if (vineCollider != null) vineCollider.enabled = true;

        // Active damage period
        yield return new WaitForSeconds(activeTime);

        // Disable damage
        canDamage = false;
        if (vineCollider != null) vineCollider.enabled = false;

        // Play retract animation
        animator.SetTrigger("Retract");

        // Play retract particles
        if (retractParticles != null)
        {
            retractParticles.Play();
        }

        // Wait for retraction animation (adjust time to match your animation)
        yield return new WaitForSeconds(0.3f);

        // Destroy vine
        Destroy(gameObject);
    }

    // Animation event called at peak growth frame
    public void AE_GrowthComplete()
    {
        // Additional logic when growth finishes
    }

    // Animation event called when retraction starts
    public void AE_RetractionStart()
    {
        // Additional logic when retraction begins
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (canDamage && other.CompareTag("Player"))
        {
            other.GetComponent<Player>().TakeDamage(damage);
            if (preventMultiHits) canDamage = false;
        }
    }

    // For debugging
    private void OnDrawGizmos()
    {
        if (vineCollider != null)
        {
            Gizmos.color = canDamage ? Color.red : Color.yellow;
            Gizmos.DrawWireCube(transform.position, vineCollider.bounds.size);
        }
    }
}