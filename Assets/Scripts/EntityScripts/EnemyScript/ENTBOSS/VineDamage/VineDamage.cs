using UnityEngine;

public class VineDamage : MonoBehaviour
{
    [Header("Damage")]
    [SerializeField] private float damage = 10f;
    [SerializeField] private float activeTime = 2f; // Damage window

    private Animator animator;
    private Collider2D vineCollider;
    private bool hasDamaged;

    private void Start()
    {
        animator = GetComponent<Animator>();
        vineCollider = GetComponent<Collider2D>();
        vineCollider.enabled = false; // Start inactive

        // Activate collider mid-animation (adjust 0.5f to match your keyframe)
        Invoke(nameof(ActivateCollider), animator.GetCurrentAnimatorStateInfo(0).length * 0.5f);

        // Destroy when animation ends
        Destroy(gameObject, animator.GetCurrentAnimatorStateInfo(0).length);
    }

    private void ActivateCollider()
    {
        vineCollider.enabled = true;
        Invoke(nameof(DeactivateCollider), activeTime); // Disable after damage window
    }

    private void DeactivateCollider()
    {
        vineCollider.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && vineCollider.enabled && !hasDamaged)
        {
            other.GetComponent<Player>().TakeDamage(damage);
            hasDamaged = true;
        }
    }
}