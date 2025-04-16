using System.Collections;
using UnityEngine;

public class Player : Entity
{
    [Header("Components")]
    public Weapon currentWeapon;
    public PlayerMovement playerMovement;
    private AudioManager audioManager;

    [Header("Settings")]
    [SerializeField] private float invulnerabilityDuration = 1f;
    private bool isInvulnerable = false;

    [Header("Health Sync")]
    [SerializeField] private bool usePlayerStats = true;
    private bool statsInitialized = false;


    private playerHealth healthUI;


    protected override void Awake()
    {
        // Initialize Entity components first
        base.Awake();

        // Get other components
        currentWeapon = GetComponentInChildren<Weapon>();
        playerMovement = GetComponent<PlayerMovement>();
        audioManager = GameObject.FindGameObjectWithTag("Audio")?.GetComponent<AudioManager>();

        // Initialize health system
        InitializeHealthSystem();
    }

    private void InitializeHealthSystem()
    {
        if (!usePlayerStats) return;

        if (PlayerStats.Instance == null)
        {
            Debug.LogWarning("PlayerStats not found! Using local health values.");
            statsInitialized = false;
        }
        else
        {
            maxHealth = PlayerStats.Instance.maxhealth;
            health = PlayerStats.Instance.health;
            statsInitialized = true;
            Debug.Log("PlayerStats health initialized: " + health);
        }
    }

    public void Attack()
    {
        if (currentWeapon != null && !isDead)
        {
            playerMovement.TriggerAttackAnimation();
            audioManager?.PlayWeaponSFX(currentWeapon.weaponType);
        }
    }

    public override void TakeDamage(float damage)
    {
        TakeDamage(damage, Vector2.zero);
    }

    public void TakeDamage(float damage, Vector2 direction)
    {
        if (isDead || isInvulnerable) return;

        // Handle damage
        if (statsInitialized)
        {
            PlayerStats.Instance.health -= damage;
            health = PlayerStats.Instance.health;
        }
        else
        {
            health -= damage;
        }

        // 🔊 Play hit sound effect
        audioManager?.PlaySFX(audioManager.playerIsHitSFX);

        // Visual feedback
        animator.SetFloat("HitDirectionX", direction.x);
        animator.SetFloat("HitDirectionY", direction.y);
        animator.Play("Hit", 0, 0f);

        if (health <= 0) Die();
        else StartCoroutine(InvulnerabilityPeriod());
    }

    private IEnumerator InvulnerabilityPeriod()
    {
        isInvulnerable = true;
        float flashInterval = 0.1f;
        float elapsed = 0f;

        while (elapsed < invulnerabilityDuration)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;
            yield return new WaitForSeconds(flashInterval);
            elapsed += flashInterval;
        }

        spriteRenderer.enabled = true;
        isInvulnerable = false;
    }

    public bool IsInvulnerable()
    {
        return isInvulnerable;
    }

    protected override void Die()
    {
        playerMovement.enabled = false;
        base.Die();
    }
}