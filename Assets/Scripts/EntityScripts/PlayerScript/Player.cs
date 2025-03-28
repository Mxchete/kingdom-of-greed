using UnityEngine;

public class Player : Entity
{
    public Weapon currentWeapon; // Reference to the equipped weapon
    public PlayerMovement playerMovement; // Links to PlayerMovement for animations

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
        }
    }

    protected override void Die()
    {
        base.Die();
    }
}