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

    protected override void Awake()
    {
        base.Awake();
        currentWeapon = GetComponentInChildren<Weapon>();
        playerMovement = GetComponent<PlayerMovement>();
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    public void Attack()
    {
        if (currentWeapon != null && !isDead)
        {
            playerMovement.TriggerAttackAnimation();
            audioManager.PlayWeaponSFX(currentWeapon.weaponType);
            // currentWeapon.Attack();
        }
    }

    public override void TakeDamage(float damage)
    {
        if (isDead || isInvulnerable) return;

        base.TakeDamage(damage);
        StartCoroutine(InvulnerabilityPeriod());
        // audioManager.PlaySFX(audioManager.playerHurt);
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

    protected override void Die()
    {
        // audioManager.PlaySFX(audioManager.playerDeath);
        playerMovement.enabled = false;
        base.Die();
    }
}