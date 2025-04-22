using UnityEngine;
using UnityEngine.InputSystem;

public class VineDamage : MonoBehaviour
{
    [Header("Damage")]
    [SerializeField] private float damage = 10f;
    [SerializeField] private float activeTime = 2f; // Damage window

    private Animator animator;
    private Collider2D vineCollider;
    private bool hasDamaged;
    private PlayerStats PlayerManager;
    private playerHealth playerHealthComponent;

    private void Start()
    {
        animator = GetComponent<Animator>();
        vineCollider = GetComponent<Collider2D>();
        vineCollider.enabled = false; // Start inactive
        PlayerManager = FindAnyObjectByType<PlayerStats>();
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
        Debug.Log("Vine touched: " + other.name);

        if (!vineCollider.enabled || hasDamaged) return;

        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            if (player == null)
            {
                Debug.LogError("Player script not found on: " + other.name);
                return;
            }

            playerHealthComponent = player.GetComponent<playerHealth>();
            if (playerHealthComponent == null)
            {
                Debug.LogError("playerHealth component not found on: " + other.name);
                return;
            }

            if (!player.IsInvulnerable())
            {
                Debug.Log("Vine hit the player!");

                player.TakeDamage(damage);
                playerHealthComponent.UpdateHealth(-damage);
                hasDamaged = true;

                Debug.Log($"Health after vine hit: {PlayerManager.health}");
            }
            else
            {
                Debug.Log("Player was invulnerable.");
            }
        }

    }
}
