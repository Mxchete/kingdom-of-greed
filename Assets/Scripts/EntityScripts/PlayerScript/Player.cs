using UnityEngine;

public class Player : Entity
{
    public Weapon currentWeapon; // Reference to the equipped weapon
    public PlayerMovement playerMovement; // Links to PlayerMovement for animations


    [Header("Audio")]
    AudioManager audioManager;

    protected override void Awake()
    {
        base.Awake();
        // Initialize weapon
        currentWeapon = GetComponentInChildren<Weapon>();
        playerMovement = GetComponent<PlayerMovement>();
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();

    }

    public void Attack()
    {
        if (currentWeapon != null)
        {
            playerMovement.TriggerAttackAnimation(); // Trigger attack animation
            audioManager.PlayWeaponSFX(currentWeapon.weaponType);
        }
    }

    protected override void Die()
    {
        base.Die();
    }
}