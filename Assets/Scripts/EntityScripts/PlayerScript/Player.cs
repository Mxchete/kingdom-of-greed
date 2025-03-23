using UnityEngine;

public class Player : Entity
{
    public Weapon currentWeapon; // Reference to the equipped weapon
    public PlayerMovement playerMovement; // Reference to PlayerMovement for animations

    protected override void Awake()
    {
        base.Awake();
        // Initialize weapon
        currentWeapon = GetComponentInChildren<Weapon>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    public void Attack()
    {
        if (currentWeapon != null)
        {
            playerMovement.TriggerAttackAnimation(); // Trigger attack animation
            // Add weapon-specific attack logic here if needed
        }
    }

    protected override void Die()
    {
        Debug.Log("Player has died!");
        base.Die();
    }
}